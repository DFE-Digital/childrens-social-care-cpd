module.exports = function (migration) {
    const details = migration
        .createContentType("details")
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
            .validations([
                {
                    unique: true,
                },
            ])
            .disabled(false)
            .omitted(false);
        
            details
                .createfield("detailsText")
                .name("Summary text")
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
                            "hyperlink"
                        ],
                        message:
                            "Only heading 1, heading 2, heading 3, heading 4, heading 5, heading 6, ordered list, unordered list, horizontal rule, quote, block entry, asset, table, link to asset, inline entry, link to entry, and link to Url nodes are allowed"
                    },
                    {
                        nodes: {}
                    }
                ])
                .disabled(false)
                .omitted(false);
              }