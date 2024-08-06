import contentful from 'contentful-management';
import core from '@actions/core';
import chalk from 'chalk';

const red = chalk.bold.red;
const managementToken = process.env.MANAGEMENT_TOKEN;
const stagingEnvironment = process.env.STAGING_ENVIRONMENT;
const spaceId = process.env.SPACE_ID;
const migrationFilename = process.env.MIGRATION_FILENAME

try {
    if (!managementToken) throw new Error("Environment variable MANAGEMENT_TOKEN not set");
    if (!stagingEnvironment) throw new Error("Environment variable STAGING_ENVIRONMENT not set");
    if (!spaceId) throw new Error("Environment variable SPACE_ID not set");
    if (!migrationFilename) throw new Error("Environment variable MIGRATION_FILENAME not set");
}
catch (e) {
    core.setFailed(red(e));
    process.exit();
}

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
    core.setFailed (red('Migration version record missing or not unique'));
    process.exit();
}

var versionEntry = entries.items[0];

versionEntry.fields.version['en-US'] = newVersion;

await client.entry.update({
    environmentId: stagingEnvironment,
    spaceId: spaceId,
    entryId: versionEntry.sys.id
}, versionEntry);

const updatedEntry = await client.entry.get({
    spaceId: spaceId,
    environmentId: stagingEnvironment,
    entryId: versionEntry.sys.id,
});

await client.entry.publish({
    environmentId: stagingEnvironment,
    spaceId: spaceId,
    entryId: versionEntry.sys.id
}, updatedEntry);
