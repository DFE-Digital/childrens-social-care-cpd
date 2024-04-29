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

console.log('total environments: ' + environments.items.length);
const actualEnvironments = environments.items.filter(obj => !("aliasedEnvironment" in obj.sys));
const environmentsInUse = actualEnvironments.length;
console.log('actual environments: ' + environmentsInUse);
console.log('space capacity: ' + spaceCapacity);
if (environmentsInUse >= spaceCapacity) {
    core.setFailed('Contentful space has insufficient environment capacity.  Configured max capacity is ' + spaceCapacity + ', current environment count is ' + environmentsInUse);
}