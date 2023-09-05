import { test, expect } from '@playwright/test'

test.describe('Experienced Practitioners', () => {

    test('User journey from homepage @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByRole('link', { name: 'Practitioners', exact: true }).click()
        await page.getByRole('link', { name: 'Develop your social work practice', exact: true }).click()

        await expect(page.locator('h1', { hasText: /^Develop your social work practice$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/develop-social-work-practice/)
        await expect(page.locator('#mmi-career')).toHaveClass(/dfe-header__navigation-item--current/)
    })

    test('User journey via menu @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByLabel('Menu').getByRole('link', { name: 'Career stage', exact: true }).click()
        await page.getByRole('link', { name: 'Practitioners', exact: true }).click()
        await page.getByRole('link', { name: 'Develop your social work practice', exact: true }).click()

        await expect(page.locator('h1', { hasText: /^Develop your social work practice$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/develop-social-work-practice/)
        await expect(page.locator('#mmi-career')).toHaveClass(/dfe-header__navigation-item--current/)
    })

    test.describe('External links @external', () => {
        const links = [
            ['Find out more about the types of CPD you can do', 'https://www.socialworkengland.org.uk/cpd/what-counts-as-cpd/'],
            ['Find out more about CPD requirements', 'https://www.socialworkengland.org.uk/cpd/'],
            ['Social work post-qualifying standards', 'https://www.gov.uk/government/publications/knowledge-and-skills-statements-for-child-and-family-social-work'],
        ]
        
        for (const link of links) {
            test(`'${link[0]}' goes to ${link[1]}`, async ({ page }) => {
                await page.goto('/develop-social-work-practice')
                var promise = page.waitForResponse(`**${link[1]}`)
                await page.getByRole('link', { name: link[0], exact: true }).click()
                var response = await promise
                
                expect(response.ok()).toBeTruthy()
            })
        }
    })
})