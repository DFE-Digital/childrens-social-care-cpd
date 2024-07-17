module.exports = async function (migration, { makeRequest }) {

  const contentTypeId = 'content',
    categoryFieldId = 'category',
    employerStandardsString = 'Employer standards';

  const response = await makeRequest({
    method: 'GET',
    url: `/content_types?sys.id[in]=${contentTypeId}`
  });

  var update = false;
  const validations = response.items[0].fields
    .filter(field => field.id == categoryFieldId)[0]
    .validations.map(rule => {
      if (rule.in && !rule.in.includes(employerStandardsString)) {
          rule.in.push(employerStandardsString);
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
