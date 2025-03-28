module.exports = async (migration) => {

  const contentTypeId = "contentLink";

  migration
    .editContentType(contentTypeId)
    .createField("icon")
    .name("Icon")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([
      {
        in: ['Signpost icon', 'Compass icon', 'None']
      }
    ])
    .disabled(false)
    .omitted(false);
    
  migration
    .editContentType(contentTypeId)
    .changeFieldControl("icon", "builtin", "radio", {
      helpText:
          'You can select an icon to appear before the text of the content link. If left blank, no icon will render.',
      });
    
};
