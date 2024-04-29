import contentful from 'contentful-management';
import core from '@actions/core';

const managementToken = process.env.MANAGEMENT_TOKEN;
const spaceId = process.env.SPACE_ID;
const spaceCapacity = process.env.SPACE_CAPACITY;

const client = contentful.createClient({
    accessToken: managementToken
}, { type: 'plain' })

const environments = await client.environment.getMany({
    spaceId: spaceId
});

const actualEnvironments = environments.items.filter(e => !("aliasedEnvironment" in e.sys));
const environmentsInUse = actualEnvironments.length;

if (environmentsInUse >= spaceCapacity) {
    core.setFailed('Contentful space has insufficient environment capacity.  Configured max capacity is ' + spaceCapacity + ', current environment count is ' + environmentsInUse);
}