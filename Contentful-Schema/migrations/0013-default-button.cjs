module.exports = function (migration) {
  const defaultButton = migration
    .createContentType("defaultButton")
    .name("Default button")
    .description(
      "A green button for the main call to action on a page, e.g. 'Next'."
    )
    .displayField("buttonText");
  defaultButton
    .createField("buttonText")
    .name("Button text")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);
  defaultButton
    .createField("relativeLink")
    .name("Relative link")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);
  defaultButton.changeFieldControl("buttonText", "builtin", "singleLine", {});

  defaultButton.changeFieldControl("relativeLink", "builtin", "singleLine", {
    helpText:
      "The relative link for the page you are linking to - e.g. /employer-standards/standard-1/overview",
  });
};
