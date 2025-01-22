module.exports = async function (migration) {

  const contentTypeId = 'pathwaysModuleSection';

  migration
    .editContentType(contentTypeId)
    .changeFieldControl("name", "builtin", "singleLine", {
      helpText:
        "This will appear in the header bar and will also be useful for identifying the section in Contentful. E.g. Pathway 1: Introduction to this pathway, Pathway 1: Providing clarity, etc.",
    });
};
