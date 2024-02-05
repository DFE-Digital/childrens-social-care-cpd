import boxen from "boxen"
import yargs from "yargs"
import { hideBin } from "yargs/helpers"

import chalk from "chalk"
import contentful from 'contentful-management'

console.log(boxen(chalk.whiteBright("Switch Alias environment to a new version"), {padding: 1}));
const argv = await yargs(hideBin(process.argv))
    .usage('Usage: $0 [options]')
    .describe("a", "The environment alias to be switched to a new version")
    .alias("a", "alias")
    .boolean("rollback")
    .alias("r", "rollback")
    .help(true)
    .version(false)
    .example("$0 -a master", "Switch the master alias to the next (calculated) version of the environment it points to")
    .example("$0 -a master -r", "Rollback the master alias to the previous (calculated) version")
    .demandOption(["a"])
    .argv;

const token = process.env.CONTENTFUL_MANAGEMENT_TOKEN as string
if (!token) throw new Error("No Contentful management API key specified, make sure you've set the CONTENTFUL_MANAGEMENT_ACCESS_TOKEN environment variable")

const spaceId = process.env.CPD_SPACE_ID as string
if (!spaceId) throw new Error("No space specified, make sure you've set the CPD_SPACE_ID environment variable")

const aliasId = argv.a as string
if (!aliasId) throw new Error("No alias specified")

console.log("Creating Contentful client")
const client = contentful.createClient({ accessToken: token })

console.log(`Fetching space`)
const space = await client.getSpace(spaceId)

console.log(`Fetching environment alias ${chalk.yellowBright(aliasId)}`)
const alias = await space.getEnvironmentAlias(argv.a as string)

console.log(`Fetching alias's target environment ${chalk.yellowBright(alias.environment.sys.id)}`)
const environment = await space.getEnvironment(alias.environment.sys.id)

console.log("Generating new environment name")

console.log(`Current Environment name is ${chalk.yellowBright(environment.name)}`)
const tokens = environment.name.split("__")
if (tokens === undefined) {
    throw new Error(`${chalk.redBright("Error:")} Environment ${chalk.yellowBright(environment.sys.id)} is not in the correct format`)
}

let version = tokens.length > 1 ? parseInt(tokens[1]) : 1

let newEnvName = ''

if (argv.rollback) {
    console.log("Rolling back version")
    newEnvName = `${tokens[0]}__${version-1}`
    if (version === 2) {
        newEnvName = `${tokens[0]}`
    }
} else {
    console.log("Versioning for a release")
    newEnvName = `${tokens[0]}__${version+1}`
}

//const newEnvName = `${tokens[0]}__${version+1}`
console.log(`New environment name will be ${chalk.yellowBright(newEnvName)}`)

console.log(`Checking that ${chalk.yellowBright(newEnvName)} exists`)
const newEnvironment = await space.getEnvironment(newEnvName)

console.log(`Switching alias ${chalk.yellowBright(aliasId)} to ${chalk.yellowBright(newEnvironment.name)}`)
alias.environment.sys.id = newEnvironment.sys.id
await alias.update()

console.log("Done")