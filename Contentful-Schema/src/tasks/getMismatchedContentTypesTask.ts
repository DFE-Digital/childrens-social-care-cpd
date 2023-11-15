import { ContentType } from 'contentful-management'
import { delay } from 'listr2'
import { ContentfulContext } from './contentfulSetupTask.js'
import chalk from 'chalk'

interface ResyncContext {
    contentfulCtx : ContentfulContext
    contentTypes : ContentType[]
}

export const getMismatchedContentTypesTask = {
    title: "Verifying Content Model EditorInterface Controls order matches that of the Fields ",
    task: async (ctx: ResyncContext, task: any): Promise<void> => {
        const contentTypes = await ctx.contentfulCtx.environment!.getContentTypes()
        for (const contentType of contentTypes.items) {
            task.output = `Checking ${chalk.yellowBright(contentType.sys.id)} (${contentType.name})`

            const fields = contentType.fields;
            const editorInterface = await contentType.getEditorInterface()
            const controls = editorInterface.controls

            if (fields.length != controls?.length) {
                throw new Error(`${chalk.redBright("Error:")} Content Type ${chalk.yellowBright(contentType.sys.id)} has a mismatchc between the number of fields and number of editor controls`)
            }

            for (let index=0; index<fields.length; index++) {
                const field = fields[index]
                const control = controls[index]

                if (field.id !== control.fieldId) {
                    ctx.contentTypes.push(contentType)
                    break;
                }
            }

            await delay(200)
        }

        if (ctx.contentTypes.length > 0) {
            throw new Error(chalk.redBright("Mismatched Content Types detected"))
        }
    }
}