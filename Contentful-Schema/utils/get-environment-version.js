import contentful from 'contentful';
import chalk from 'chalk';

const error = chalk.bold.red;

var client = contentful.createClient({
    accessToken: process.env.DELIVERY_KEY,
    space: process.env.SPACE_ID,
    environment: process.env.ENVIRONMENT
});

try {
    client.getEntries({ content_type: 'migrationVersion' }).then(entries => {
        if (entries.total == 0) {
            console.error (error('migrationVersion content type exists, but no migration version record found. Environment inconsistently prepared.'));
            process.exit(1);
        }
        if (entries.total > 1) {
            console.error (error('More than one migration version record found.'));
            process.exit(1);
        }
        var item = entries.items[0];
        console.log(item.fields.version);
    });
} catch (x) {
    console.error(error('migrationVersion content type not found - environment not prepared for migrations?'));
    process.exit(1);
}
