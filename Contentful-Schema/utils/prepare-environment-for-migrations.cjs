module.exports = function (migration) {
    const contentType = migration
        .createContentType('migrationVersion')
        .name('Migration Version');

    contentType.createField('version')
        .name('Version')
        .type('Integer')
        .defaultValue({ 'en-US': 0});
}