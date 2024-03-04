import { Listr  } from "listr2"
import boxen from "boxen"
import yargs from "yargs"
import { hideBin } from "yargs/helpers"

import { ContentfulContext } from '../tasks/contentfulSetupTask.js'
import { contentfulSetupTask } from '../tasks/contentfulSetupTask.js'
import chalk from "chalk"
import { ContentType } from "contentful-management"
import { getMismatchedContentTypesTask } from "../tasks/getMismatchedContentTypesTask.js"
import { fixMismatchedContentTypesTask } from "../tasks/fixMismatchedContentTypesTask.js"

console.log(boxen(chalk.whiteBright("Resynchronise an EditorInterface Controls with the order of its the Content Types Fields"), {padding: 1}));

const argv = await yargs(hideBin(process.argv))
    .usage('Usage: $0 [options]')
    .describe("e", "ID the environment in the destination space")
    .alias("e", "environment")
    .describe("o", "Change the output format")
    .alias("o", "output-format")
    .choices('o', ["default", "simple"])
    .default("o", "default")
    .help(false)
    .version(false)
    .demandOption(["e"])
    .argv

argv.t = process.env.CONTENTFUL_MANAGEMENT_ACCESS_TOKEN
argv.s = process.env.CPD_SPACE_ID

if (!argv.t) {
    console.error(`${chalk.redBright("Contentful management API token is required, please set the")} ${chalk.yellowBright.bold("CONTENTFUL_MANAGEMENT_ACCESS_TOKEN")} ${chalk.redBright("environment variable")}`)
    process.exit(1)
}

if (!argv.s) {
    console.error(`${chalk.redBright("Contentful space id is required, please set the")} ${chalk.yellowBright.bold("CPD_SPACE_ID")} ${chalk.redBright("environment variable")}`)
    process.exit(1)
}

interface MainContext {
    contentfulCtx: ContentfulContext
    contentTypes: ContentType[]
}

console.log(chalk.whiteBright(`Space is ${chalk.yellowBright(argv.s)}`))
console.log(chalk.whiteBright(`Environment is ${chalk.yellowBright(argv.e)}`))

const ctx: MainContext = {
    contentfulCtx: {
        params: {
            managementToken: argv.t as string,
            space: argv.s as string,
            environment: argv.e as string
        }
    },
    contentTypes: []
}

const options = {
    concurrent: false, 
    ctx,
    renderer: argv.o as string
}

const tasks = new Listr<MainContext>(
  [
    {
        task: (ctx, task): Listr => {
            return task.newListr<ContentfulContext>(
                [ contentfulSetupTask ],
                { ctx : ctx.contentfulCtx}
            )
        }
    },
    {
        task: (ctx, task): Listr => {
            return task.newListr<MainContext>(
                [ getMismatchedContentTypesTask ],
                { ctx }
            )
        },
        exitOnError: false
    },
    {
        skip: (ctx): boolean => ctx.contentTypes.length === 0,
        task: (ctx, task): Listr => {
            return task.newListr<MainContext>(
                [ fixMismatchedContentTypesTask ],
                { ctx }
            )
        }
    }
  ],
  options as unknown as any
)

try {
    await tasks.run()
} catch (e) {
    console.error(e)
}