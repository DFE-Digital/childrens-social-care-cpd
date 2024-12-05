module.exports = async (migration) => {

  const contentTypeId = "infoBox";

  migration
    .editContentType(contentTypeId)
    .createField("type")
    .name("Type")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([
      {
        in: ['Blue "i" icon', 'Green "i" icon', 'Green "brain" icon'],
      },
    ])
    .disabled(false)
    .omitted(false);
    
  migration
    .editContentType(contentTypeId)
    .changeFieldControl("type", "builtin", "radio", {
      helpText:
          'How should the info box appear on screen?  If left blank will default to the blue "i" icon',
      });
    
};
