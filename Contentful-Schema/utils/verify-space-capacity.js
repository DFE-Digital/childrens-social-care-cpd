import contentful from 'contentful-management';
import core from '@actions/core';
import chalk from 'chalk';

const red = chalk.bold.red;
const managementToken = process.env.MANAGEMENT_TOKEN;
const spaceId = process.env.SPACE_ID;
const spaceCapacity = process.env.SPACE_CAPACITY;

try {
    if (!managementToken) throw new Error("Environment variable MANAGEMENT_TOKEN not set");
    if (!spaceId) throw new Error("Environment variable SPACE_ID not set");
    if (!spaceCapacity) throw new Error("Environment variable SPACE_CAPACITY not set");
}
catch (e) {
    core.setFailed(red(e));
    process.exit();
}

const client = contentful.createClient({
    accessToken: managementToken
}, { type: 'plain' })

const environments = await client.environment.getMany({
    spaceId: spaceId
});

const actualEnvironments = environments.items.filter(obj => !("aliasedEnvironment" in obj.sys));
const environmentsInUse = actualEnvironments.length;

if (environmentsInUse >= spaceCapacity) {
    core.setFailed('Contentful space has insufficient environment capacity.  Configured max capacity is ' + spaceCapacity + ', current environment count is ' + environmentsInUse);
}