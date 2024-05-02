import contentful from 'contentful';
import chalk from 'chalk';
import core from '@actions/core';

const red = chalk.bold.red;
const spaceId = process.env.SPACE_ID;
const contentfulAlias = process.env.ENVIRONMENT;
const deliveryKey = process.env.DELIVERY_KEY;

try {
    if (!deliveryKey) throw new Error("Environment variable DELIVERY_KEY not set");
    if (!spaceId) throw new Error("Environment variable SPACE_ID not set");
    if (!contentfulAlias) throw new Error("Environment variable ENVIRONMENT not set");
}
catch (e) {
    core.setFailed(red(e));
    process.exit();
}

var client = contentful.createClient({
    accessToken: deliveryKey,
    space: spaceId,
    environment: contentfulAlias
});

client.getEntries({ content_type: 'migrationVersion' }).then(entries => {

    if (entries.total == 0) {
        core.setFailed(red('migrationVersion content type exists, but no migration version record found. Environment inconsistently prepared.'));
        process.exit(1);
    }
    if (entries.total > 1) {
        core.setFailed(red('More than one migration version record found.'));
        process.exit(1);
    }
    var migrationVersion = entries.items[0].fields.version;
    core.setOutput('migration-version', migrationVersion);

}).catch(x => {
    core.setFailed(red('migrationVersion content type not found - environment not prepared for migrations?'));
});
