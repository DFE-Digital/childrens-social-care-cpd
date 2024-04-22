module.exports = async function (migration) {
    const contentType = migration
        .createContentType('mb-test')
        .name("Matt's Test Content Type")
        .description("A dummy content type used for testing migration flow")
        .displayField('title');

    contentType.createField('title')
        .name('Title')
        .type('Symbol');
}