import { test, expect } from '@playwright/test'

test.describe('Pathway 2: middle managers', () => {
    test('User journey from homepage @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByRole('link', { name: 'Experienced practitioners', exact: true }).click()
        await page.getByRole('link', { name: 'Pathway 2: middle managers', exact: true }).click()

        await expect(page.locator('h1', { hasText: /^Pathway 2: middle managers$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/pathway-2/)
        await expect(page.locator('#mmi-developmentProgrammes')).toHaveClass(/dfe-header__navigation-item--current/)
    })

    test('User journey from homepage - 2 @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByRole('link', { name: 'Managers', exact: true }).click()
        await page.getByRole('link', { name: 'Pathway 2: middle managers', exact: true }).click()

        await expect(page.locator('h1', { hasText: /^Pathway 2: middle managers$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/pathway-2/)
        await expect(page.locator('#mmi-developmentProgrammes')).toHaveClass(/dfe-header__navigation-item--current/)
    })

    test('User journey via Career menu @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByLabel('Menu').getByRole('link', { name: 'Career stages', exact: true }).click()
        await page.getByRole('link', { name: 'Experienced practitioners', exact: true }).click()
        await page.getByRole('link', { name: 'Pathway 2: middle managers', exact: true }).click()

        await expect(page.locator('h1', { hasText: /^Pathway 2: middle managers$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/pathway-2/)
        await expect(page.locator('#mmi-developmentProgrammes')).toHaveClass(/dfe-header__navigation-item--current/)
    })

    test('User journey via Development menu @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByLabel('Menu').getByRole('link', { name: 'Development programmes', exact: true }).click()
        await page.getByRole('link', { name: 'Pathway 2: middle managers', exact: true }).click()

        await expect(page.locator('h1', { hasText: /^Pathway 2: middle managers$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/pathway-2/)
        await expect(page.locator('#mmi-developmentProgrammes')).toHaveClass(/dfe-header__navigation-item--current/)
    })

    test.describe('External links @external', () => {
        const links = [
            ['Frontline', 'https://thefrontline.org.uk/develop-your-career/pathways-programme/'],
            ['Frontline\'s network of leaders', 'https://thefrontline.org.uk/develop-your-career/frontline-fellowship/'],
            //['Find out more about Pathway 2 including how to apply', 'https://thefrontline.org.uk/develop-your-career/pathways-programme/'], // Currently closed
            ['Social work post-qualifying standards', 'https://www.gov.uk/government/publications/knowledge-and-skills-statements-for-child-and-family-social-work'],
        ]
        
        for (const link of links) {
            test(`'${link[0]}' goes to ${link[1]}`, async ({ page }) => {
                await page.goto('/pathway-2')
                var promise = page.waitForResponse(`**${link[1]}`)
                await page.getByRole('link', { name: link[0], exact: true }).click()
                var response = await promise
                
                expect(response.ok()).toBeTruthy()
            })
        }
    })
})