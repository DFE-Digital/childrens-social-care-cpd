module.exports = async function (migration) {

  const contentTypeId = 'pathwaysModuleSection';

  migration
    .editContentType(contentTypeId)
    .createField("shortName")
    .name("Module Section Short Name")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);

  migration
    .editContentType(contentTypeId)
    .changeFieldControl("shortName", "builtin", "singleLine", {
      helpText:
        "This will appear in the Table of Contents so does not need to include the Pathway number. E.g. Introduction to this pathway, Providing clarity, etc. If left blank, the Table of Contents will default to the full Module Section Name.",
    });

  migration
    .editContentType(contentTypeId)
    .changeFieldControl("name", "builtin", "singleLine", {
      helpText:
        "This will appear in the header bar and will also be useful for identifying the section in Contentful. E.g. Pathway 1: Introduction to this pathway, Pathway 1: Providing clarity, etc.",
    });
};
