module.exports = async function (migration, { makeRequest }) {

  const contentTypeId = "content";

  migration
    .editContentType(contentTypeId)
    .createField("showPrintThisPage")
    .name("Show Print this Page")
    .type("Boolean")
    .localized(false)
    .required(false)
    .validations([])
    .defaultValue({
      "en-US": true,
    })
    .disabled(false)
    .omitted(false);

  migration
    .editContentType(contentTypeId)
    .changeFieldControl("showPrintThisPage", "builtin", "boolean", {
      helpText:
        "Should this page include a Print this Page component?  It requires that a Feedback component is included in the page's items.",
      trueLabel: "Yes",
      falseLabel: "No",
    });
};
