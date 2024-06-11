module.exports = async function (migration) {
    /*
        ! S U P E R  I M P O R T A N T !

        Replace this migration with the first "real" migration that gets created.

        Do NOT allow this migration to be run into live environments.

        (It isn't dangerous or anything, we just don't want unnecessary stuff in the Contentful environments)
    */
    const contentType = migration
        .createContentType('example')
        .name("Example Content Type")
        .description("An example content type used for demonstrating migration flow")
        .displayField('title');

    contentType.createField('title')
        .name('Title')
        .type('Symbol');
}