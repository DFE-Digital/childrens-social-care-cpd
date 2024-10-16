module.exports = async function (migration, { makeRequest }) {

/*
* Add fields required for breadcrumbs to Content content type
*/

  const contentTypeId = "content";

  migration.editContentType(contentTypeId)
    .createField("parentPages")
    .name("Parent Page(s)")
    .type("Array")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false)
    .items({
      type: "Link",

      validations: [
        {
          linkContentType: ["content"],
        },
      ],

      linkType: "Entry",
    });

  migration.editContentType(contentTypeId)
    .changeFieldControl("parentPages", "builtin", "entryLinksEditor", {
      helpText:
        'What pages are parent pages to this page.  Any page which should display breadcrumbs should have at least one parent page. Breadcrumbs will only display on a page with Show Content Header set to Yes.',
      bulkEditing: false,
      showLinkEntityAction: true,
      showCreateEntityAction: true,
    });
  
  migration.editContentType(contentTypeId)
    .createField("breadcrumbText")
    .name("Breadcrumb Text")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);

  migration.editContentType(contentTypeId)
    .changeFieldControl("breadcrumbText", "builtin", "singleLine", {
      helpText:
        "Text to represent this page in a breadcrumb trail. Optional - Page Title will be used instead if left blank.",
    });
};
