module.exports = async function (migration) {
  const details = migration
    .createContentType("details")
    .name("Details")
    .description(
      "Allows users to view more detailed information if they need it, as per the GDS component of the same name."
    )
    .displayField("summaryText");

  details
    .createField("summaryText")
    .name("Summary text")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);

  details
    .createField("detailsText")
    .name("Details text")
    .type("RichText")
    .localized(false)
    .required(true)
    .validations([
      {
        enabledMarks: [
          "bold",
          "italic",
          "underline",
          "code",
          "superscript",
          "subscript",
        ],
        message:
          "Only bold, italic, underline, code, superscript, and subscript marks are allowed",
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

  details.changeFieldControl("summaryText", "builtin", "singleLine", {});
  details.changeFieldControl("detailsText", "builtin", "richTextEditor", {});

  const contentTypeId = "content",
    linkingFieldId = "items",
    detailsTypeId = "details";

  const response = await makeRequest({
    method: "GET",
    url: `/content_types?sys.id[in]=${contentTypeId}`,
  });

  const validations = response.items[0].fields
    .filter((field) => field.id == linkingFieldId)[0]
    .items.validations.map((rule) => {
      if (
        rule.linkContentType &&
        !rule.linkContentType.includes(detailsTypeId)
      ) {
        rule.linkContentType.push(detailsTypeId);
      }
      return rule;
    });

  migration.editContentType(contentTypeId).editField(linkingFieldId).items({
    type: "Link",
    linkType: "Entry",
    validations: validations,
  });
};
