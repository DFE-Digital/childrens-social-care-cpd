module.exports = async function (migration, { makeRequest }) {

  const quoteBox = migration
    .createContentType("quoteBox")
    .name("Quote Box")
    .description("Allows display of quotes in a distinctive bordered box.")
    .displayField("name");
  quoteBox
    .createField("name")
    .name("Name")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);

  quoteBox
    .createField("quoteText")
    .name("Quote Text")
    .type("RichText")
    .localized(false)
    .required(false)
    .validations([
      {
        enabledMarks: [
          "bold",
          "italic",
          "underline",
          "code",
          "superscript",
          "subscript"
        ],

        message:
          "Only bold, italic, underline, code, superscript, subscript, and strikethrough marks are allowed",
      },
      {
        enabledNodeTypes: [
          "heading-1",
          "heading-2",
          "heading-3",
          "heading-4",
          "heading-5",
          "heading-6",
          "ordered-list",
          "unordered-list",
          "hr",
          "blockquote",
          "embedded-entry-block",
          "embedded-asset-block",
          "table",
          "asset-hyperlink",
          "embedded-entry-inline",
          "entry-hyperlink",
          "hyperlink",
        ],

        message:
          "Only heading 1, heading 2, heading 3, heading 4, heading 5, heading 6, ordered list, unordered list, horizontal rule, quote, block entry, asset, table, link to asset, inline entry, link to entry, and link to Url nodes are allowed",
      },
      {
        nodes: {},
      },
    ])
    .disabled(false)
    .omitted(false);

  quoteBox.changeFieldControl("name", "builtin", "singleLine", {});
  quoteBox.changeFieldControl("quoteText", "builtin", "richTextEditor", {});


  /*
  * Add quoteBox to list of content types allowed in content pages
  */
  var contentTypeId = "content",
    linkingFieldId = "items",
    quoteBoxTypeId = "quoteBox";

  var response = await makeRequest({
    method: "GET",
    url: `/content_types?sys.id[in]=${contentTypeId}`,
  });

  var validations = response.items[0].fields
    .filter((field) => field.id == linkingFieldId)[0]
    .items.validations.map((rule) => {
      if (
        rule.linkContentType &&
        !rule.linkContentType.includes(quoteBoxTypeId)
      ) {
        rule.linkContentType.push(quoteBoxTypeId);
      }
      return rule;
    });

  migration.editContentType(contentTypeId).editField(linkingFieldId).items({
    type: "Link",
    linkType: "Entry",
    validations: validations,
  });

  /*
  * Add quoteBox to list of content types allowed in accordion sections
  */
  contentTypeId = "accordionSection";
  linkingFieldId = "content";

  response = await makeRequest({
    method: "GET",
    url: `/content_types?sys.id[in]=${contentTypeId}`,
  });

  validations = response.items[0].fields
    .filter((field) => field.id == linkingFieldId)[0]
    .items.validations.map((rule) => {
      if (
        rule.linkContentType &&
        !rule.linkContentType.includes(quoteBoxTypeId)
      ) {
        rule.linkContentType.push(quoteBoxTypeId);
      }
      return rule;
    });

  migration.editContentType(contentTypeId).editField(linkingFieldId).items({
    type: "Link",
    linkType: "Entry",
    validations: validations,
  });
};
