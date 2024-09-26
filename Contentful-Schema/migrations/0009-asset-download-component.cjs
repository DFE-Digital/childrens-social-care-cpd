module.exports = async function (migration, { makeRequest }) {

  /*
  * Create assetDownload content type
  */
  const assetDownload = migration
    .createContentType("assetDownload")
    .name("Asset Download")
    .description("")
    .displayField("linkText");
    
  assetDownload
    .createField("linkText")
    .name("Link Text")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);

  assetDownload
    .createField("asset")
    .name("Asset")
    .type("Link")
    .localized(false)
    .required(true)
    .validations([
      {
        linkMimetypeGroup: ["richtext", "spreadsheet", "pdfdocument"],
      },
    ])
    .disabled(false)
    .omitted(false)
    .linkType("Asset");

  assetDownload.changeFieldControl("linkText", "builtin", "singleLine", {});
  assetDownload.changeFieldControl("asset", "builtin", "assetLinkEditor", {});

  /*
  * Add assetDownload to list of content types allowed in content pages
  */

  const contentTypeId = "content",
    linkingFieldId = "items",
    assetDownloadTypeId = "assetDownload";

  const response = await makeRequest({
    method: "GET",
    url: `/content_types?sys.id[in]=${contentTypeId}`,
  });

  const validations = response.items[0].fields
    .filter((field) => field.id == linkingFieldId)[0]
    .items.validations.map((rule) => {
      if (
        rule.linkContentType &&
        !rule.linkContentType.includes(assetDownloadTypeId)
      ) {
        rule.linkContentType.push(assetDownloadTypeId);
      }
      return rule;
    });

  migration.editContentType(contentTypeId).editField(linkingFieldId).items({
    type: "Link",
    linkType: "Entry",
    validations: validations,
  });
};
