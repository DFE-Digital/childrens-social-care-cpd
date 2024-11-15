module.exports = async function (migration, { makeRequest }) {
  const embeddedVideo = migration
    .createContentType("embeddedVideo")
    .name("Embedded Video")
    .description("Container for embedding a video from YouTube, Vimeo")
    .displayField("name");

  embeddedVideo
    .createField("name")
    .name("Name")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([
      {
        unique: true,
      },
    ])
    .disabled(false)
    .omitted(false);

  embeddedVideo
    .createField("embeddingCode")
    .name("Embedding Code")
    .type("Text")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);

  embeddedVideo.changeFieldControl("name", "builtin", "singleLine", {
    helpText:
      "Name used to refer to the embedded video internally. This will not display in the website.",
  });

  embeddedVideo.changeFieldControl("embeddingCode", "builtin", "markdown", {
    helpText:
      "The embedding code provided by the video's host (YouTube, Vimeo)",
  });

  /*
  * add the new component to the list of items allowed on a page
  */
  var contentTypeId = 'content',
    linkingFieldId = 'items',
    embeddedVideoTypeId = 'embeddedVideo';

  var response = await makeRequest({
    method: 'GET',
    url: `/content_types?sys.id[in]=${contentTypeId}`
  });

  var validations = response.items[0].fields
    .filter(field => field.id == linkingFieldId)[0]
    .items.validations.map(rule => {
        if (rule.linkContentType && !rule.linkContentType.includes(embeddedVideoTypeId)) {
            rule.linkContentType.push(embeddedVideoTypeId)
        }
        return rule;
    });

  migration.editContentType(contentTypeId)
    .editField(linkingFieldId).items({
        type: 'Link',
        linkType: 'Entry',
        validations: validations,
    });

  /*
  * add the new component to the list of items allowed within an accordion section
  */
  contentTypeId = 'accordionSection';
  linkingFieldId = 'content';

  response = await makeRequest({
    method: 'GET',
    url: `/content_types?sys.id[in]=${contentTypeId}`
  });

  validations = response.items[0].fields
    .filter(field => field.id == linkingFieldId)[0]
    .items.validations.map(rule => {
        if (rule.linkContentType && !rule.linkContentType.includes(embeddedVideoTypeId)) {
            rule.linkContentType.push(embeddedVideoTypeId)
        }
        return rule;
    });

  migration.editContentType(contentTypeId)
    .editField(linkingFieldId).items({
        type: 'Link',
        linkType: 'Entry',
        validations: validations,
    });
  };
