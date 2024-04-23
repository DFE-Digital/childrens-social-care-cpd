module.exports = async function (migration) {
    const contentType = migration
        .createContentType('migrationVersion')
        .name('Migration Version')
        .description('Should contain a single entry which denotes the schema migration version number most recently applied to the environment.')
        .displayField('version');

    contentType.createField('version')
        .name('Version')
        .type('Integer')
        .defaultValue({ 'en-US': 0});
}