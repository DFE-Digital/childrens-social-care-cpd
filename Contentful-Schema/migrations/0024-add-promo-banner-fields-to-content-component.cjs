module.exports = async (migration) => {

  const contentTypeId = "content";

  // Create Promo Banner header field
  migration
    .editContentType(contentTypeId)
    .createField("promoBannerHeader")
    .name("Home Page Promo Banner Header")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);

// Create Promo Banner subheading field
    migration
    .editContentType(contentTypeId)
    .createField("promoBannerSubheading")
    .name("Home Page Promo Banner Subheading")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);

// Create Promo Banner column layout field

    migration
    .editContentType(contentTypeId)
    .createField("promoBannerColumnLayout")
    .name("Home Page Promo Banner Column Layout")
    .type("Link")
    .linkType("Entry")
    .localized(false)
    .required(false)
    .validations([
      {
        "linkContentType": 
        [
          "columnLayout"
        ]
      }])
    .disabled(false)
    .omitted(false);
    
  migration
    .editContentType(contentTypeId)
    .changeFieldControl("promoBannerHeader", "builtin", "singleLine", {
      helpText:
          'This will render in a light blue banner directly below the Hero banner.',
      });
    
    migration
      .editContentType(contentTypeId)
      .changeFieldControl("promoBannerSubheading", "builtin", "singleLine", {
        helpText:
            'This will appear beneath the promo banner header.',
      });
    
    migration
      .editContentType(contentTypeId)
      .changeFieldControl("promoBannerColumnLayout", "builtin", "reference", {
        helpText:
            'Add a Column Layout component to begin adding Content Links to the promo banner.',
      });
    
};
