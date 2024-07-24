module.exports = async function (migration, { makeRequest }) {

    /*
        Update the Image Card component to work well in the Accordion
    */
    const imageCardContentTypeId = 'imageCard',
    imageCard = migration.editContentType(imageCardContentTypeId);

    // delete the Image Side field
    imageCard.deleteField('imageSide');

    // create new Text Position field to replace 'Image Side'
    imageCard.createField('textPosition')
        .name('Text Position')
        .type('Symbol')
        .localized(false)
        .required(true)
        .validations([
            {
                in: ['Left', 'Right', 'None'],
            },
        ])
        .disabled(false)
        .omitted(false)
        .defaultValue({
            "en-US": 'None'
        });

    // make Text Position a radio button control
    imageCard.changeFieldControl('textPosition', 'radio', 'builtin');

    // make the Text field of an Image Card optional
    imageCard.editField('text', {
        'required': false
    });

    /*
        Create the Accordion Section content type
    */
    const accordionSection = migration
        .createContentType("accordionSection")
        .name("Accordion Section")
        .description(
          "A single expandable section of an accordion. May contain rich text, infobox, links or images"
        )
        .displayField("name");
    
    accordionSection
        .createField("name")
        .name("Name")
        .type("Symbol")
        .localized(false)
        .required(true)
        .validations([
          {
            unique: true,
          },
        ])
        .disabled(false)
        .omitted(false);
    
    accordionSection
        .createField("heading")
        .name("Accordion Section Heading")
        .type("Symbol")
        .localized(false)
        .required(true)
        .validations([])
        .disabled(false)
        .omitted(false);

    accordionSection
        .createField("summaryLine")
        .name("Accordion Section Summary Line")
        .type("Symbol")
        .localized(false)
        .required(false)
        .validations([])
        .disabled(false)
        .omitted(false);
        
    accordionSection
        .createField("content")
        .name("Accordion Section Content")
        .type("Array")
        .localized(false)
        .required(true)
        .validations([])
        .disabled(false)
        .omitted(false)
        .items({
          type: "Link",
    
          validations: [
            {
              linkContentType: [
                "contentLink",
                "imageCard",
                "infoBox",
                "richTextBlock",
                "textBlock",
              ],
            },
          ],
    
          linkType: "Entry",
        });
    
    accordionSection.changeFieldControl("name", "builtin", "singleLine", {});
    accordionSection.changeFieldControl("heading", "builtin", "singleLine", {});
    accordionSection.changeFieldControl(
        "summaryLine",
        "builtin",
        "singleLine",
        {}
    );
    accordionSection.changeFieldControl(
        "content",
        "builtin",
        "entryLinksEditor",
        {}
    );
    
    /*
        Create the Accordion content type
    */
    const accordion = migration
        .createContentType("accordion")
        .name("Accordion")
        .description(
          "An accordion contains a collection of Accordion Section entries."
        )
        .displayField("name");
    
    accordion
        .createField("name")
        .name("Name")
        .type("Symbol")
        .localized(false)
        .required(true)
        .validations([
          {
            unique: true,
          },
        ])
        .disabled(false)
        .omitted(false);
    
    accordion
        .createField("sections")
        .name("Sections")
        .type("Array")
        .localized(false)
        .required(true)
        .validations([])
        .disabled(false)
        .omitted(false)
        .items({
          type: "Link",
    
          validations: [
            {
              linkContentType: ["accordionSection"],
            },
          ],
    
          linkType: "Entry",
        });
    
    accordion.changeFieldControl("name", "builtin", "singleLine", {});
    accordion.changeFieldControl("sections", "builtin", "entryLinksEditor", {}); 

    /*
        Add Accordion content types to list of controls allowed on Content pages
    */
    const contentTypeId = 'content',
        linkingFieldId = 'items',
        accordionTypeId = 'accordion';

    const response = await makeRequest({
        method: 'GET',
        url: `/content_types?sys.id[in]=${contentTypeId}`
    });

    const validations = response.items[0].fields
        .filter(field => field.id == linkingFieldId)[0]
        .items.validations.map(rule => {
            if (rule.linkContentType && !rule.linkContentType.includes(accordionTypeId)) {
                rule.linkContentType.push(accordionTypeId)
            }
            return rule;
        });

    migration.editContentType(contentTypeId)
        .editField(linkingFieldId).items({
            type: 'Link',
            linkType: 'Entry',
            validations: validations,
        });  

}