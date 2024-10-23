module.exports = async function (migration, { makeRequest }) {
  const creditsBlock = migration
    .createContentType("creditsBlock")
    .name("Credits Block")
    .description(
      "Shows who developed a resource, the date it was published and the date it was last updated."
    );

  creditsBlock
    .createField("developerOfResource")
    .name("Developer of resource")
    .type("RichText")
    .localized(false)
    .required(true)
    .validations([
      {
        enabledMarks: [],
        message: "Marks are not allowed",
      },
      {
        enabledNodeTypes: ["hyperlink"],
        message: "Only link to Url nodes are allowed",
      },
      {
        nodes: {},
      },
    ])
    .disabled(false)
    .omitted(false);

  creditsBlock
    .createField("secondaryDevelopersOfResource")
    .name("Secondary developer(s) of resource")
    .type("RichText")
    .localized(false)
    .required(false)
    .validations([
      {
        enabledMarks: [],
        message: "Marks are not allowed",
      },
      {
        enabledNodeTypes: ["hyperlink"],
        message: "Only link to Url nodes are allowed",
      },
      {
        nodes: {},
      },
    ])
    .disabled(false)
    .omitted(false);

  creditsBlock
    .createField("datePublished")
    .name("Date published")
    .type("Date")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);
  creditsBlock
    .createField("dateLastUpdated")
    .name("Date last updated")
    .type("Date")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);

  creditsBlock.changeFieldControl(
    "developerOfResource",
    "builtin",
    "richTextEditor",
    {
      helpText:
        "Who created this resource? You can make their name a hyperlink to their website if appropriate.",
    }
  );

  creditsBlock.changeFieldControl(
    "secondaryDevelopersOfResource",
    "builtin",
    "richTextEditor",
    {
      helpText:
        "Optional: Include anyone that this resource was created in collaboration with. E.g. 'DfE'.",
    }
  );

  creditsBlock.changeFieldControl("datePublished", "builtin", "datePicker", {
    ampm: "24",
    format: "dateonly",
    helpText: "What date was this resource first published on?",
  });

  creditsBlock.changeFieldControl("dateLastUpdated", "builtin", "datePicker", {
    ampm: "24",
    format: "dateonly",
    helpText: "When was this resource last updated?",
  });
};
