module.exports = function (migration) {
    const infoBox = migration
        .createContentType("infoBox")
        .name("Info Box")
        .description(
            "A text box with a light blue shaded background and an 'i' information icon in the top left corner. It is used to draw attention to a particular piece of text."
        )
        .displayField("title");

    infoBox
        .createField("title")
        .name("Title")
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

    infoBox
        .createField("displayTitle")
        .name("Display Title")
        .type("Boolean")
        .localized(false)
        .required(true)
        .validations([])
        .defaultValue({
            "en-US": false,
        })
        .disabled(false)
        .omitted(false);

    infoBox
        .createField("titleLevel")
        .name("Title Level")
        .type("Integer")
        .localized(false)
        .required(false)
        .validations([
            {
                in: [1, 2, 3],
            },
        ])
        .defaultValue({
            "en-US": 2,
        })
        .disabled(false)
        .omitted(false);

    infoBox
        .createField("document")
        .name("Document")
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
                    "hyperlink",
                    "entry-hyperlink",
                    "asset-hyperlink",
                    "embedded-entry-inline",
                ],

                message:
                    "Only heading 1, heading 2, heading 3, heading 4, heading 5, heading 6, ordered list, unordered list, horizontal rule, quote, block entry, asset, table, link to Url, link to entry, link to asset, and inline entry nodes are allowed",
            },
            {
                nodes: {
                    "embedded-entry-block": [
                        {
                            linkContentType: ["roleList"],
                            message: null,
                        },
                    ],

                    "embedded-entry-inline": [
                        {
                            linkContentType: ["contentLink"],
                            message: null,
                        },
                    ],
                },
            },
        ])
        .disabled(false)
        .omitted(false);

    infoBox.changeFieldControl("title", "builtin", "singleLine", {});

    infoBox.changeFieldControl("displayTitle", "builtin", "boolean", {
        helpText:
            "Indicates whether the content should be displayed with the Title as a heading",
        trueLabel: "Yes",
        falseLabel: "No",
    });

    infoBox.changeFieldControl("titleLevel", "builtin", "radio", {
        helpText: "The importance of the title, 1 is the most important",
    });

    infoBox.changeFieldControl("document", "builtin", "richTextEditor", {});
};