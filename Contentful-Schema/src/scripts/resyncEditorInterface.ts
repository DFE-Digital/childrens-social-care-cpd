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
    .describe("t", "Contentful management API token")
    .alias("t", "management-token")
    .describe("s", "ID of the destination space ")
    .alias("s", "space")
    .describe("e", "ID the environment in the destination space")
    .alias("e", "environment")
    .describe("o", "Change the output format")
    .alias("o", "output-format")
    .choices('o', ["default", "simple"])
    .default("o", "default")
    .help(false)
    .version(false)
    .demandOption(["t", "s", "e"])
    .argv;

// const argv = {
//     t: process.env.CONTENTFUL_MANAGEMENT_ACCESS_TOKEN,
//     s: process.env.CPD_SPACE_ID,
//     e: "dev",
//     o: "default",
// }

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
  {
    concurrent: false, 
    ctx,
    renderer: argv.o as string
  }
)

try {
    await tasks.run()
} catch (e) {
    console.error(e)
}