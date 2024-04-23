import contentful from 'contentful-management';
import core from '@actions/core';
import chalk from 'chalk';

const managementToken = process.env.MANAGEMENT_TOKEN;
const stagingEnvironment = process.env.STAGING_ENVIRONMENT;
const spaceId = process.env.SPACE_ID;
const migrationFilename = process.env.MIGRATION_FILENAME
const newVersion = parseInt(migrationFilename.split('-')[0]);

var client = contentful.createClient(
    { accessToken:  managementToken },
    { type: 'plain' }
);

var entries = await client.entry.getMany({
    query: {
        content_type: "migrationVersion",
    },
    environmentId: stagingEnvironment,
    spaceId: spaceId
});

if (entries.total !== 1) {
    core.setFailed (chalk.red('Migration version record missing or not unique'));
    process.exit();
}

var versionEntry = entries.items[0];

versionEntry.fields.version['en-US'] = newVersion;

client.entry.update({
    environmentId: stagingEnvironment,
    spaceId: spaceId,
    entryId: versionEntry.sys.id
}, versionEntry);
