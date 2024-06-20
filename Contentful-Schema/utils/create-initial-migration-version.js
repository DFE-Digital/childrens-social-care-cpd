import contentful from 'contentful-management';
import chalk from 'chalk';
import core from '@actions/core';

const red = chalk.bold.red;
const managementToken = process.env.MANAGEMENT_TOKEN;
const spaceId = process.env.SPACE_ID;
const contentfulAlias = process.env.ENVIRONMENT;

try {
    if (!managementToken) throw new Error("Environment variable MANAGEMENT_TOKEN not set");
    if (!spaceId) throw new Error("Environment variable SPACE_ID not set");
    if (!contentfulAlias) throw new Error("Environment variable ENVIRONMENT not set");
}
catch (e) {
    core.setFailed(red(e));
    process.exit();
}

var client = contentful.createClient(
    { accessToken: managementToken },
    { type: 'plain' }
);

const newMigrationVersionEntry = await client.entry.create ({
    environmentId: contentfulAlias,
    spaceId: spaceId,
    contentTypeId: 'migrationVersion'
}, {})
    
await client.entry.publish({
    environmentId: contentfulAlias,
    spaceId: spaceId,
    entryId: newMigrationVersionEntry.sys.id
}, newMigrationVersionEntry);