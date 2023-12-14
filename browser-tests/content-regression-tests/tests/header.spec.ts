import { test, expect } from '@playwright/test'

test.describe('Header', () => {

    test.beforeEach(async ({ page }) => {
        await page.goto('/')
    })

    const links = [
        ['Home', '/home'],
        ['Career stages', '/career-stages'],
        ['Development programmes', '/development-programmes'],
        ['Explore roles', '/explore-roles'],
        //['Resources and learning', '/resources-learning'],
    ]

    for (const link of links) {

        test(`Contains nav link ${link[0]} that goes to ${link[1]}`, async ({ page }) => {
            var promise = page.waitForResponse(`**${link[1]}`)
            await page.getByLabel('Menu').getByRole('link', { name: link[0], exact: true }).click()
            var response = await promise
            
            expect(response.ok()).toBeTruthy()
        })

    }

})