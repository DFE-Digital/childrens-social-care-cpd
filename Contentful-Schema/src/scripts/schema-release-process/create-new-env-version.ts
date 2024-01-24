import boxen from "boxen"
import yargs from "yargs"
import { hideBin } from "yargs/helpers"

import chalk from "chalk"
import contentful from 'contentful-management'

console.log(boxen(chalk.whiteBright("Clone an Alias target environment to a new version"), {padding: 1}));
const argv = await yargs(hideBin(process.argv))
    .usage('Usage: $0 [options]')
    .describe("s", "The environment to be cloned into a new version")
    .alias("s", "source-environment")
    .describe("t", "The target alias")
    .alias("t", "target-alias")
    .help(false)
    .version(false)
    .demandOption(["s", "t"])
    .argv

const token = process.env.CONTENTFUL_MANAGEMENT_TOKEN as string
if (!token) throw new Error("No Contentful management API key specified, make sure you've set the CONTENTFUL_MANAGEMENT_ACCESS_TOKEN environment variable")

const spaceId = process.env.CPD_SPACE_ID as string
if (!spaceId) throw new Error("No space specified, make sure you've set the CPD_SPACE_ID environment variable")

const sourceEnvironmentName = argv.s as string
if (!sourceEnvironmentName) throw new Error("No alias specified")

const targetAliasId = argv.t as string

console.log("Creating Contentful client")
const client = contentful.createClient({ accessToken: token })

console.log(`Fetching space`)
const space = await client.getSpace(spaceId)

console.log(`Fetching list of all environments`)
const allEnvironments = await space.getEnvironments()

console.log(`Locating source environment ${chalk.yellowBright(sourceEnvironmentName)}`)
const environment = allEnvironments.items.find(e => e.name === sourceEnvironmentName)

if (!environment) {
    throw new Error(`Source environment ${chalk.yellowBright(sourceEnvironmentName)} not found`)
}

console.log(`Fetching target alias ${chalk.yellowBright(targetAliasId)}`)
const targetAlias = await space.getEnvironmentAlias(targetAliasId)

console.log(`Fetching alias's target environment ${chalk.yellowBright(targetAlias.environment.sys.id)}`)
const currentEnvironment = allEnvironments.items.find(e => e.sys.id === targetAlias.environment.sys.id)

if (!currentEnvironment) {
    throw new Error(`Alias's target environment ${chalk.yellowBright(targetAlias.environment.sys.id)} not found`)
}

console.log("Generating new environment name")
console.log(`Current Environment name is ${chalk.yellowBright(currentEnvironment.name)}`)
const tokens = environment.name.split("__")
if (tokens === undefined) {
    throw new Error(`${chalk.redBright("Error:")} Environment ${chalk.yellowBright(environment.sys.id)} is not in the correct format`)
}

let version = tokens.length > 1 ? parseInt(tokens[1]) : 1
const newEnvName = `${currentEnvironment.name}__${version+1}`

console.log(`New environment name will be ${chalk.yellowBright(newEnvName)}`)

console.log(`Checking if environment ${chalk.yellowBright(newEnvName)} already exists`)
if (allEnvironments.items.find(e => e.name === newEnvName)) {
    throw new Error(`Next release environment ${chalk.yellowBright(newEnvName)} already exists`)
}

console.log(`Cloning new environment ${chalk.yellowBright(newEnvName)} from ${chalk.yellowBright(environment.name)}`)
await space.createEnvironmentWithId(newEnvName, { name: newEnvName }, environment.name)

console.log("Done")