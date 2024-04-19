import contentful from 'contentful-management';

var client = contentful.createClient(
    { accessToken: process.env.MANAGEMENT_TOKEN },
    { type: 'plain' }
);

const newMigrationVersionEntry = await client.entry.create ({
    environmentId: process.env.ENVIRONMENT,
    spaceId: process.env.SPACE_ID,
    contentTypeId: 'migrationVersion'
}, {})
    
await client.entry.publish({
    environmentId: process.env.ENVIRONMENT,
    spaceId: process.env.SPACE_ID,
    entryId: newMigrationVersionEntry.sys.id
}, newMigrationVersionEntry);