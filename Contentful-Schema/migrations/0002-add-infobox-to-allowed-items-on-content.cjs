module.exports = async (migration, { makeRequest }) => {

    const contentTypeId = 'content',
        linkingFieldId = 'items',
        infoBoxTypeId = 'infoBox';

    const response = await makeRequest({
        method: 'GET',
        url: `/content_types?sys.id[in]=${contentTypeId}`
    });

    const validations = response.items[0].fields
        .filter(field => field.id == linkingFieldId)[0]
        .items.validations.map(rule => {
            if (rule.linkContentType && !rule.linkContentType.includes(infoBoxTypeId)) {
                rule.linkContentType.push(infoBoxTypeId)
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
