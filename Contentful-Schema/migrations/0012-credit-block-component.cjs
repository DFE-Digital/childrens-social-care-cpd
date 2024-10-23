module.exports = async function (migration, { makeRequest }) {
  const creditBlock = migration
    .createContentType("creditBlock")
    .name("Credit Block")
    .description(
      "Shows who developed a resource, the date it was published and the date it was last updated."
    );

  creditBlock
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

  creditBlock
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

  creditBlock
    .createField("datePublished")
    .name("Date published")
    .type("Date")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);
  creditBlock
    .createField("dateLastUpdated")
    .name("Date last updated")
    .type("Date")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);

  creditBlock.changeFieldControl(
    "developerOfResource",
    "builtin",
    "richTextEditor",
    {
      helpText:
        "Who created this resource? You can make their name a hyperlink to their website if appropriate.",
    }
  );

  creditBlock.changeFieldControl(
    "secondaryDevelopersOfResource",
    "builtin",
    "richTextEditor",
    {
      helpText:
        "Optional: Include anyone that this resource was created in collaboration with. E.g. 'DfE'.",
    }
  );

  creditBlock.changeFieldControl("datePublished", "builtin", "datePicker", {
    ampm: "24",
    format: "dateonly",
    helpText: "What date was this resource first published on?",
  });

  creditBlock.changeFieldControl("dateLastUpdated", "builtin", "datePicker", {
    ampm: "24",
    format: "dateonly",
    helpText: "When was this resource last updated?",
  });
};
