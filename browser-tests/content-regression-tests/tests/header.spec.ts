import { test, expect } from '@playwright/test'

test.describe('Header @header', () => {

    test.beforeEach(async ({ page }) => {
        await page.goto('/')
    })

    const links = [
        ['Home', '/home'],
        ['Career stages', '/career-stages'],
        ['Development programmes', '/development-programmes'],
        ['Explore roles', '/explore-roles'],
    ]

    for (const link of links) {
        test(`Header contains nav link ${link[0]} that goes to ${link[1]}`, async ({ page }) => {
            await page.getByLabel('Menu').getByRole('link', { name: link[0], exact: true }).click()
            
            await expect(page).toHaveURL(new RegExp(`${link[1]}`))
        })
    }

})