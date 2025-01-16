module.exports = async function (migration) {

  const contentTypeId = 'pathwaysModuleSection';

  migration
    .editContentType(contentTypeId)
    .createField("shortName")
    .name("Module Section Table of Contents Name")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);

  migration
    .editContentType(contentTypeId)
    .changeFieldControl(
      "moduleSectionTableOfContentsName",
      "builtin",
      "singleLine",
      {
        helpText:
          "How the module section will be named on the Table of Contents. Optional (if left blank will default to the Module Section Name)",
      }
    );
};
