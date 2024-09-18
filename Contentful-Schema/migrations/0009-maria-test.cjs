module.exports = async function (migration) {
  const contentType = migration
    .createContentType("maria-test")
    .name("Maria Test Content Type")
    .description("A dummy content type used for testing migration flow")
    .displayField("title");
  contentType.createField("title").name("Title").type("Symbol");
};
