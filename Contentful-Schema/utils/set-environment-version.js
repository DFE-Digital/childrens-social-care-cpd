import contentful from 'contentful-management';
import minimist from 'minimist';

const argv = minimist(process.argv.slice(2));
const newVersion = parseInt(argv.migrationFilename.split('-')[0]);

var client = contentful.createClient(
    {
        accessToken: argv.token,
    },
    {
     type: 'plain'
    }
);


var entries = await client.entry.getMany({
    query: {
        content_type: "migrationVersion",
    },
    environmentId: argv.environment,
    spaceId: argv.space
});

if (entries.total !== 1) {
    console.error ('Migration version record missing or not unique');
}

var versionEntry = entries.items[0];

versionEntry.fields.version['en-US'] = newVersion;

client.entry.update({
    environmentId: argv.environment,
    spaceId: argv.space,
    entryId: versionEntry.sys.id
}, versionEntry);
