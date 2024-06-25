module.exports = async function (migration, { makeRequest }) {
    const contentsAnchor = migration
    .createContentType("contentsAnchor")
    .name("Contents Anchor")
    .description(
      "An invisible anchor tag that is referenced by a Page Contents component"
    )
    .displayField("anchorTag");

  contentsAnchor
    .createField("anchorTag")
    .name("anchor-tag")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([
      {
        unique: true,
      },
      {
        regexp: {
          pattern: "[a-z]*",
          flags: null,
        },
      },
    ])
    .disabled(false)
    .omitted(false);

  contentsAnchor.changeFieldControl("anchorTag", "builtin", "singleLine", {});

  const pageContentsItem = migration
    .createContentType("pageContentsItem")
    .name("Page Contents Item")
    .description("An individual item in a Page Contents component")
    .displayField("itemText");

  pageContentsItem
    .createField("itemText")
    .name("Item Text")
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

  pageContentsItem
    .createField("anchorLink")
    .name("Anchor Link")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);
  pageContentsItem.changeFieldControl("itemText", "builtin", "singleLine", {});
  pageContentsItem.changeFieldControl(
    "anchorLink",
    "builtin",
    "singleLine",
    {}
  );

  const pageContents = migration
    .createContentType("pageContents")
    .name("Page Contents")
    .description(
      "A page contents component linking to Content Anchors in the page"
    )
    .displayField("name");

  pageContents
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

  pageContents
    .createField("displayText")
    .name("Display Text")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([])
    .defaultValue({
      "en-US": "Contents on this page",
    })
    .disabled(false)
    .omitted(false);

  pageContents
    .createField("contentLinks")
    .name("Content Links")
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
          linkContentType: ["pageContentsItem"],
        },
      ],

      linkType: "Entry",
    });

  pageContents.changeFieldControl("name", "builtin", "singleLine", {});
  pageContents.changeFieldControl("displayText", "builtin", "singleLine", {});
  pageContents.changeFieldControl(
    "contentLinks",
    "builtin",
    "entryLinksEditor",
    {}
  );


  const contentTypeId = 'content',
  linkingFieldId = 'items',
  contentsAnchorTypeId = 'contentsAnchor',
  pageContentsTypeId = 'pageContents';

const response = await makeRequest({
  method: 'GET',
  url: `/content_types?sys.id[in]=${contentTypeId}`
});

const validations = response.items[0].fields
  .filter(field => field.id == linkingFieldId)[0]
  .items.validations.map(rule => {
    if (rule.linkContentType && !rule.linkContentType.includes(contentsAnchorTypeId)) {
        rule.linkContentType.push(contentsAnchorTypeId)
    }
    if (rule.linkContentType && !rule.linkContentType.includes(pageContentsTypeId)) {
        rule.linkContentType.push(pageContentsTypeId)
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