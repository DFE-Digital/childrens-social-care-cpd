module.exports = async function (migration, { makeRequest }) {
  const creditBlock = migration
    .createContentType("creditBlock")
    .name("Credit Block")
    .description(
      "Shows who developed a resource, the date it was published and the date it was last updated."
    )
    .displayField("name");

  creditBlock
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

  creditBlock.changeFieldControl(
    "name",
    "builtin",
    "singleLine",
    {
      helpText:
        "Name is only for internal reference, and will not display on website",
    }
  );

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

};
