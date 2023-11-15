import contentful, { ClientAPI, Environment, Space } from 'contentful-management'

export interface ContentfulParams {
    managementToken: string
    space: string
    environment: string
}

export interface ContentfulContext {
    params: ContentfulParams
    client?: ClientAPI
    space?: Space
    environment?: Environment
}

export const contentfulSetupTask = {
    title: "Initiating Contentful",
    task: async (ctx: ContentfulContext, task: any): Promise<void> => {
        task.output = "Create Contentful client"
        ctx.client = contentful.createClient({ accessToken: ctx.params.managementToken })
        
        task.output = "Fetching space"
        ctx.space = await ctx.client.getSpace(ctx.params.space)

        task.output = "Fetching environment"
        ctx.environment = await ctx.space.getEnvironment(ctx.params.environment)
    }
}