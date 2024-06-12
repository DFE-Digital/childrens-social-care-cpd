module.exports = function (migration) {
  const content = migration
    .createContentType("content")
    .name("Content")
    .description("A content container")
    .displayField("id");

  content
    .createField("id")
    .name("Id")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([
      {
        regexp: {
          pattern: "^[0-9a-z](\\/?[0-9a-z\\-])*$",
          flags: null,
        },

        message:
          "Please only use lower case letters, dash (-) or forward slash (/) separated words and numbers. Ids can only begin with letters or numbers. e.g. 'roles-list', 'roles/practitioners-1'",
      },
      {
        unique: true,
      },
    ])
    .disabled(false)
    .omitted(false);

  content
    .createField("contentType")
    .name("Content Type")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([
      {
        in: ["Resource"],
      },
    ])
    .disabled(false)
    .omitted(false);

  content
    .createField("title")
    .name("Page Title")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);

  content
    .createField("estimatedReadingTime")
    .name("Estimated Reading Time")
    .type("Integer")
    .localized(false)
    .required(false)
    .validations([
      {
        range: {
          min: 0,
        },
      },
    ])
    .defaultValue({
      "en-US": 0,
    })
    .disabled(false)
    .omitted(false);

  content
    .createField("showContentHeader")
    .name("Show Content Header")
    .type("Boolean")
    .localized(false)
    .required(true)
    .validations([])
    .defaultValue({
      "en-US": false,
    })
    .disabled(false)
    .omitted(false);

  content
    .createField("contentTitle")
    .name("Content Title")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);
  content
    .createField("contentSubtitle")
    .name("Content Subtitle")
    .type("Text")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);
  content
    .createField("searchSummary")
    .name("Search Summary")
    .type("Text")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);

  content
    .createField("category")
    .name("Category")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([
      {
        in: [
          "Home",
          "Career information",
          "Development programmes",
          "Explore roles",
          "Resources",
        ],
      },
    ])
    .disabled(false)
    .omitted(false);

  content
    .createField("backLink")
    .name("Back Link")
    .type("Link")
    .localized(false)
    .required(false)
    .validations([
      {
        linkContentType: ["contentLink"],
      },
    ])
    .disabled(false)
    .omitted(false)
    .linkType("Entry");

  content
    .createField("items")
    .name("Items")
    .type("Array")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false)
    .items({
      type: "Link",

      validations: [
        {
          linkContentType: [
            "columnLayout",
            "content",
            "contentLink",
            "contentSeparator",
            "detailedRole",
            "heroBanner",
            "linkListCard",
            "richTextBlock",
            "textBlock",
            "roleList",
            "detailedPathway",
            "areaOfPracticeList",
            "areaOfPractice",
            "imageCard",
            "audioResource",
            "pdfFileResource",
            "videoResource",
            "feedback",
            "infoBox",
          ],
        },
      ],

      linkType: "Entry",
    });

  content
    .createField("navigation")
    .name("Navigation")
    .type("Link")
    .localized(false)
    .required(false)
    .validations([
      {
        linkContentType: ["navigationMenu"],
      },
    ])
    .disabled(false)
    .omitted(false)
    .linkType("Entry");

  content
    .createField("relatedContent")
    .name("Related Content")
    .type("Array")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false)
    .items({
      type: "Link",

      validations: [
        {
          linkContentType: ["contentLink"],
        },
      ],

      linkType: "Entry",
    });

  content
    .createField("mcLinkTest")
    .name("mc-link-test")
    .type("Link")
    .localized(false)
    .required(false)
    .validations([
      {
        linkContentType: ["mcPoc"],
      },
    ])
    .disabled(false)
    .omitted(false)
    .linkType("Entry");

  content.changeFieldControl("id", "builtin", "singleLine", {});

  content.changeFieldControl("contentType", "builtin", "dropdown", {
    helpText: "The type of content this Content item represents",
  });

  content.changeFieldControl("title", "builtin", "singleLine", {
    helpText:
      "The title of the content contained within this item, it will become the page title in the website",
  });

  content.changeFieldControl(
    "estimatedReadingTime",
    "builtin",
    "numberEditor",
    {
      helpText: "in minutes",
    }
  );

  content.changeFieldControl("showContentHeader", "builtin", "boolean", {});
  content.changeFieldControl("contentTitle", "builtin", "singleLine", {});
  content.changeFieldControl("contentSubtitle", "builtin", "multipleLine", {});

  content.changeFieldControl("searchSummary", "builtin", "multipleLine", {
    helpText: "This is the text that is shown in a search result",
  });

  content.changeFieldControl("category", "builtin", "radio", {});
  content.changeFieldControl("backLink", "builtin", "entryLinkEditor", {});

  content.changeFieldControl("items", "builtin", "entryLinksEditor", {
    helpText: "The content of this item",
    bulkEditing: false,
    showLinkEntityAction: true,
    showCreateEntityAction: true,
  });

  content.changeFieldControl("navigation", "builtin", "entryLinkEditor", {
    helpText:
      "Note navigation menus are only shown when this content item is the root item being viewed",
    showLinkEntityAction: true,
    showCreateEntityAction: true,
  });

  content.changeFieldControl(
    "relatedContent",
    "builtin",
    "entryLinksEditor",
    {}
  );
  content.changeFieldControl("mcLinkTest", "builtin", "entryLinkEditor", {});
};
