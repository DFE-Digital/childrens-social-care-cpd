module.exports = async function (migration, { makeRequest }) {

    const contentTypeId = 'content',
      categoryFieldId = 'category',
      agencyResourcesString = 'Agency resources';
  
    const response = await makeRequest({
      method: 'GET',
      url: `/content_types?sys.id[in]=${contentTypeId}`
    });
  
    var update = false;
    const validations = response.items[0].fields
      .filter(field => field.id == categoryFieldId)[0]
      .validations.map(rule => {
        if (rule.in && !rule.in.includes(agencyResourcesString)) {
            rule.in.push(agencyResourcesString);
            update = true;
        }
        return rule;
      });
  
    if (update) {
      migration
        .editContentType(contentTypeId)
        .editField(categoryFieldId).validations(validations);
    }
  };