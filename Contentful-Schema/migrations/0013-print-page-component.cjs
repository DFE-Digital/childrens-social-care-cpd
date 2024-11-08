module.exports = async function (migration, { makeRequest }) {

  const contentTypeId = "content";

  migration
    .editContentType(contentTypeId)
    .createField("printThisPageLocation")
    .name("Print this Page Location")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([
      {
        in: [
          "Before Feedback",
          "Before Credit Block",
          "Bottom of Page",
          "None",
        ],
      },
    ])
    .defaultValue({
      "en-US": "None",
    })
    .disabled(false)
    .omitted(false);

    migration
      .editContentType(contentTypeId)
      .changeFieldControl("printThisPageLocation", "builtin", "radio", {
      helpText: "Where should a Print this Page component be located?",
    });
  };
