import contentful from 'contentful-management';
import core from '@actions/core';
import chalk from 'chalk';

const managementToken = process.env.MANAGEMENT_TOKEN;
const stagingEnvironment = process.env.STAGING_ENVIRONMENT;
const spaceId = process.env.SPACE_ID;
const contentfulAlias = process.env.ENVIRONMENT;
const red = chalk.bold.red;

try {
    if (!managementToken) throw new Error("Environment variable MANAGEMENT_TOKEN not set");
    if (!stagingEnvironment) throw new Error("Environment variable STAGING_ENVIRONMENT not set");
    if (!spaceId) throw new Error("Environment variable SPACE_ID not set");
    if (!contentfulAlias) throw new Error("Environment variable ENVIRONMENT not set");
}
catch (e) {
    core.setFailed(red(e));
    process.exit();
}

// temp debug output
console.log('Pointing alias ' + contentfulAlias + ' at environment ' + stagingEnvironment + ' on space ' + spaceId);

const client = contentful.createClient(
    { accessToken: managementToken }
);

// get the Contentful space
const space = await client.getSpace(spaceId);

// get the alias that we're repointing
const alias = await space.getEnvironmentAlias(contentfulAlias);

// change the alias's target, and update
alias.environment.sys.id = stagingEnvironment;
await alias.update();
