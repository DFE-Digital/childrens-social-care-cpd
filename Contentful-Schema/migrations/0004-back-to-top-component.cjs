module.exports = async function (migration, { makeRequest }) {

  // Create content type
  const backToTop = migration
    .createContentType("backToTop")
    .name("Back to Top")
    .description("An in-page link to scroll the page back to the top")
    .displayField("displayText");

  backToTop
    .createField("displayText")
    .name("Display Text")
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

  backToTop
    .createField("icon")
    .name("Icon")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([
      {
        in: ["No Icon", "Arrow"],
      },
    ])
    .defaultValue({
      "en-US": "Arrow",
    })
    .disabled(false)
    .omitted(false);

  backToTop.changeFieldControl("displayText", "builtin", "singleLine", {});
  backToTop.changeFieldControl("icon", "builtin", "radio", {});

  // add new control to list of controls allowed on content pages
  const contentTypeId = 'content',
    linkingFieldId = 'items',
    backToTopTypeId = 'backToTop';

  const response = await makeRequest({
    method: 'GET',
    url: `/content_types?sys.id[in]=${contentTypeId}`
  });

  const validations = response.items[0].fields
    .filter(field => field.id == linkingFieldId)[0]
    .items.validations.map(rule => {
      if (rule.linkContentType && !rule.linkContentType.includes(backToTopTypeId)) {
          rule.linkContentType.push(backToTopTypeId)
      }
      return rule;
    });

  migration.editContentType(contentTypeId)
    .editField(linkingFieldId).items({
        type: 'Link',
        linkType: 'Entry',
        validations: validations,
    });  
};
