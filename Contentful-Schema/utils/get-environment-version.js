import contentful from 'contentful';
import minimist from 'minimist';

const argv = minimist(process.argv.slice(2));

var client = contentful.createClient({
    accessToken: argv.token,
    space: argv.space
});


client.getEntries({ content_type: 'migrationVersion' }).then(entries => {
    if(entries.total !== 1) {
        console.log('99');
//        console.error ('More than one migration version record');
    }
    var item = entries.items[0];
    console.log(item.fields.version);
});
