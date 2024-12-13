module.exports = async function (migration) {
  const pathwaysModule = migration
    .createContentType("pathwaysModule")
    .name("Pathways Module")
    .description("For example, 'Pathway 1'.")
    .displayField("name");

  pathwaysModule
    .createField("name")
    .name("Pathways Module Name")
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

  pathwaysModule
    .createField("type")
    .name("Module Type")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([
      {
        in: ["Introductory Module", "Regular Module"],
      },
    ])
    .disabled(false)
    .omitted(false);

  pathwaysModule
    .createField("overviewPage")
    .name("Overview Page")
    .type("Link")
    .localized(false)
    .required(false)
    .validations([
      {
        linkContentType: ["content"],
      },
    ])
    .disabled(false)
    .omitted(false)
    .linkType("Entry");

  pathwaysModule
    .createField("contentsPage")
    .name("Contents Page")
    .type("Link")
    .localized(false)
    .required(false)
    .validations([
      {
        linkContentType: ["content"],
      },
    ])
    .disabled(false)
    .omitted(false)
    .linkType("Entry");

  pathwaysModule
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
          linkContentType: ["pathwaysModuleSection"],
        },
      ],

      linkType: "Entry",
    });

  pathwaysModule.changeFieldControl("name", "builtin", "singleLine", {
    helpText: "What is this module called? E.g. 'Pathway 1'",
  });

  pathwaysModule.changeFieldControl("type", "builtin", "dropdown", {
    helpText: "Is this a regular or an introductory module?",
  });

  pathwaysModule.changeFieldControl(
    "overviewPage",
    "builtin",
    "entryLinkEditor",
    {
      helpText: "Select the Overview page for this module",
      showLinkEntityAction: true,
      showCreateEntityAction: false,
    }
  );

  pathwaysModule.changeFieldControl(
    "contentsPage",
    "builtin",
    "entryLinkEditor",
    {
      helpText: "Select the Table of Contents page for this module",
      showLinkEntityAction: true,
      showCreateEntityAction: false,
    }
  );

  pathwaysModule.changeFieldControl("sections", "builtin", "entryLinksEditor", {
    helpText: "Select the Module Sections for this module",
    bulkEditing: false,
    showLinkEntityAction: true,
    showCreateEntityAction: false,
  });

  // Add content type to fields on Content content type: approach 2
  const contentTypeId = "content";
  migration
    .editContentType(contentTypeId)
    .createField("pathwaysModule")
    .name("Pathways Module")
    .type("Link")
    .localized(false)
    .required(false)
    .validations([
      {
        linkContentType: ["pathwaysModule"],
      },
    ])
    .disabled(false)
    .omitted(false)
    .linkType("Entry");

  migration
    .editContentType(contentTypeId)
    .changeFieldControl("pathwaysModule", "builtin", "entryLinkEditor", {
      helpText:
        "If this page is part of a Pathways Module., select which one - e.g. 'Pathway 1'",
      showLinkEntityAction: true,
      showCreateEntityAction: false,
    });

  // Set page type on 'Content' Content Type
  migration
    .editContentType(contentTypeId)
    .createField("pageType")
    .name("Page Type")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([
      {
        in: [
          "Standard Page",
          "Pathways: Overview Page",
          "Pathways: Table of Contents",
          "Pathways: Training Content",
        ],
      },
    ])
    .disabled(false)
    .omitted(false);

  migration
    .editContentType(contentTypeId)
    .changeFieldControl("pageType", "builtin", "dropdown", {});
};
