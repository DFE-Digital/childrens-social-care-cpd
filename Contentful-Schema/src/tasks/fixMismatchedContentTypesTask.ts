import { ContentType } from 'contentful-management'
import { delay } from 'listr2'
import { ContentfulContext } from './contentfulSetupTask.js'
import chalk from 'chalk'

interface ResyncContext {
    contentfulCtx : ContentfulContext
    contentTypes : ContentType[]
}

export const fixMismatchedContentTypesTask = {
    title: "Resyncing Content Types EditorInterface Controls to Fields", 
    task: async (ctx: ResyncContext, task: any): Promise<void> => {
        for (const contentType of ctx.contentTypes) {
            const fields = contentType.fields
            const editorInterface = await contentType.getEditorInterface()
            const controls = editorInterface.controls!;

            task.output = `Fixing ${chalk.yellowBright(contentType.sys.id)} (${contentType.name})`
            let newControls = []
            
            for (let index=0; index<fields.length; index++) {
                var field = fields[index]
                var control = controls.find(x => x.fieldId === field.id)
                if (!control) {
                    throw new Error(`${chalk.redBright("Error:")} could not find control for field ${chalk.yellowBright(field.id)}`)
                }
                newControls.push(control)
            }
            editorInterface.controls = newControls;
            //await editorInterface.update()
            await delay(200)
        }
    }
}