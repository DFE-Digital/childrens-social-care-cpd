import { test, expect } from '@playwright/test'

test.describe('Upon: new directors programme', () => {
    test('User journey from homepage @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByRole('link', { name: 'Leaders', exact: true }).click()
        await page.getByRole('link', { name: 'Upon: new directors programme', exact: true }).click()

        await expect(page.locator('h1', { hasText: /^Upon: new directors programme$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/new-directors-programme/)
        await expect(page.locator('#mmi-developmentProgrammes')).toHaveClass(/dfe-header__navigation-item--current/)
    })

    test('User journey via menu @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByLabel('Menu').getByRole('link', { name: 'Career stages', exact: true }).click()
        await page.getByRole('link', { name: 'Leaders', exact: true }).click()
        await page.getByRole('link', { name: 'Upon: new directors programme', exact: true }).click()

        await expect(page.locator('h1', { hasText: /^Upon: new directors programme$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/new-directors-programme/)
        await expect(page.locator('#mmi-developmentProgrammes')).toHaveClass(/dfe-header__navigation-item--current/)
    })

    test.describe('External links @external', () => {
        const links = [
            ['Find out more about the upon: new directors programme', 'https://uponleaders.co.uk/new-directors-programme/'],
            ['Social work post-qualifying standards', 'https://www.gov.uk/government/publications/knowledge-and-skills-statements-for-child-and-family-social-work'],
        ]
        
        for (const link of links) {
            test(`'${link[0]}' goes to ${link[1]}`, async ({ page }) => {
                await page.goto('/new-directors-programme')
                var promise = page.waitForResponse(`**${link[1]}`)
                await page.getByRole('link', { name: link[0], exact: true }).click()
                var response = await promise
                
                expect(response.ok()).toBeTruthy()
            })
        }
    })
})