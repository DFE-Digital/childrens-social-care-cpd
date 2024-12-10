module.exports = function (migration) {
  const pathwaysModuleSection = migration
    .createContentType("pathwaysModuleSection")
    .name("Pathways Module Section")
    .description(
      "Create one section for each section within a single Pathways module."
    )
    .displayField("name");
  pathwaysModuleSection
    .createField("name")
    .name("Module Section Name")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);
  pathwaysModuleSection
    .createField("summary")
    .name("Module Section Summary")
    .type("Text")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);

  pathwaysModuleSection
    .createField("pages")
    .name("Pages in Module Section")
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
          linkContentType: ["content"],
        },
      ],

      linkType: "Entry",
    });

  pathwaysModuleSection.changeFieldControl("name", "builtin", "singleLine", {
    helpText:
      "e.g. Introduction, Maintaining curiosity, Providing clarity, etc.",
  });

  pathwaysModuleSection.changeFieldControl("summary", "builtin", "singleLine", {
    helpText:
      "This summary is what will be rendered under the Module Section subheading on the Table of Contents page.",
  });

  pathwaysModuleSection.changeFieldControl(
    "pages",
    "builtin",
    "entryLinksEditor",
    {
      helpText:
        "Select any pages ('Content' items) that are in this particular section",
      bulkEditing: false,
      showLinkEntityAction: true,
      showCreateEntityAction: false,
    }
  );
};
