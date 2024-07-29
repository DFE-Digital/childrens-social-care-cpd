module.exports = async function (migration) {

  const contentTypeId = 'navigationMenu';

  migration
    .editContentType(contentTypeId)
    .createField("header")
    .name("Header")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);

  migration
    .editContentType(contentTypeId)
    .createField("headerLevel")
    .name("Header Level")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([
      {
        in: ["1", "2", "3"],
      },
    ])
    .defaultValue({
      "en-US": "2",
    })
    .disabled(false)
    .omitted(false);


  migration
    .editContentType(contentTypeId)
    .changeFieldControl("header", "builtin", "singleLine", {});

  migration
    .editContentType(contentTypeId)
    .changeFieldControl("headerLevel", "builtin", "radio", {});
};
