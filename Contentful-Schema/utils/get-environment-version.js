import contentful from 'contentful';
import minimist from 'minimist';

const argv = minimist(process.argv.slice(2));

var client = contentful.createClient({
    accessToken: argv.token,
    space: argv.space
});

console.log(argv);
/*
client.getEntries({ content_type: 'migrationVersion' }).then(entries => {
    if(entries.total !== 1) {
        console.error ('More than one migration version record');
    }
    var item = entries.items[0];
    console.log(item.fields.version);
});
*/