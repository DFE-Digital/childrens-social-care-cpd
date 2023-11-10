import contentful from 'contentful-management'
import chalk from 'chalk'
import inquirer from 'inquirer'

async function sleep(milliseconds) {
    return new Promise((resolve) =>setTimeout(resolve, milliseconds));
}

function log(...args) {
    for (const arg of args) {
        if (arg === undefined) process.stdout.write("undefined")
        else process.stdout.write(arg)

        process.stdout.write(" ")
    }
}

function llog(...args) {
    console.log(...args)
}

async function confirm(text, def=false) {
    const answers = await inquirer.prompt([{
        name: "confirm",
        type: "confirm",
        message: text,
        default: def,
    }])

    return answers.confirm
}

async function selectEnvironment(environments) {
    const protectedEnvironments = new Set(["prod", "master", "pre-prod"])
    const map = new Map();
    for (const environment of environments)
    {
        if (!protectedEnvironments.has(environment.sys.id))
        {
            map.set(environment.sys.id, environment)
        }
    }

    const answer = await inquirer.prompt({
        type: 'list',
        name: 'environment',
        message: 'Run against which environment?',
        choices: Array.from(map.keys()),
        default: "dev"
    })

    const result = map.get(answer.environment)

    if (protectedEnvironments.has(answer.environment) && (!await confirm(`${chalk.yellowBright(result.sys.id)} is a ${chalk.redBright.bold("protected environment")}, are you sure?`))) {
        return null
    }

    return result
}

console.clear()

const client = contentful.createClient({ accessToken: process.env.CPD_MANAGEMENT_KEY })
const space = await client.getSpace(process.env.CPD_SPACE_ID)

llog(`Current space is: ${chalk.yellow(space.name)}`)

const environments = await space.getEnvironments()

let env = null
while (!env) {
    env = await selectEnvironment(environments.items)
}

if (!await confirm(`Selected environment is ${chalk.yellowBright(env.name)}. Last chance, continue?`)) process.exit()

console.log(`Environment is: ${chalk.yellow(env.name)}`)

const contentTypesToDelete = [
    "cookieBanner",
    "label",
    "link",
    "page",
    "pageFooter",
    "pageHeader",
    "pageNames",
    "pageType",
    "richText",
    "section", // card
    "test", //cardGroup,
]

for (const contentTypeToDelete of contentTypesToDelete) {
    llog()
    log("Fetching Content Type:", chalk.yellow(contentTypeToDelete))
    try
    {
        const contentType = await env.getContentType(contentTypeToDelete)
        llog()
        log("Fetching entries for:", chalk.yellow(contentTypeToDelete), `(${contentType.name})`, "...")
        let entries = await env.getEntries({ content_type: contentTypeToDelete, limit: 1000 })
        llog("found", chalk.yellow(entries.total))
        for (let entry of entries.items) {
            if (entry.sys.publishedVersion) {
                //log(`[${chalk.yellowBright("unpublish")}]`, entry.sys.id, "...")
                log(`[${chalk.yellowBright("unpublish")}]`, entry.sys.id, "=>")
                entry = await entry.unpublish()
                await sleep(600)
                llog("done")
            }
    
            log(`[${chalk.redBright.bold("deleting")}]`, entry.sys.id, "=>")
            entry.delete()
            await sleep(600)
            llog("done")
         }
    
         llog("Removed all data for", chalk.yellow(contentTypeToDelete))
         log(`[${chalk.redBright.bold("deleting")}] content type ${chalk.yellow(contentTypeToDelete)} (${contentType.name})`, "=>")
         await sleep(1500)
         await contentType.unpublish()
         await contentType.delete()
         llog("done")
    }
    catch (error)
    {
        if (error.name == "NotFound"){
            llog("=>", chalk.red("Content Type not found"))
        } else {
            llog(error)
        }
    }

     await sleep(500)
}
