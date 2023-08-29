import { test, expect } from '@playwright/test'

test.describe('Practitioners', () => {
    test('User journey from homepage @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByRole('link', { name: 'Practitioners', exact: true }).click()

        await expect(page.locator('h1', { hasText: /^Practitioners$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/practitioners/)
        await expect(page.locator('#mmi-career')).toHaveClass(/dfe-header__navigation-item--current/)
    })

    test('User journey via menu @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByLabel('Menu').getByRole('link', { name: 'Career information', exact: true }).click()
        await page.getByRole('link', { name: 'Practitioners', exact: true }).click()

        await expect(page.locator('h1', { hasText: /^Practitioners$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/practitioners/)
        await expect(page.locator('#mmi-career')).toHaveClass(/dfe-header__navigation-item--current/)
    })

    test.describe('Links', () => {
        const links = [
            ['Assessed and supported year in employment (ASYE)', '/asye', 'Assessed and supported year in employment \\(ASYE\\)'],
            ['View all programmes', '/development-programmes', 'Child and family social work development programmes'],
            ['Develop your social work practice', '/develop-social-work-practice', 'Develop your social work practice'],
            ['Explore all roles', '/explore-roles', 'Roles in child and family social work'],
        ]

        for (const link of links) {
            test(`Goes to the ${link[0]} page`, async ({ page }) => {
                await page.goto('/practitioners')
                await page.getByRole('link', { name: link[0], exact: true }).click()
                await expect(page).toHaveURL(new RegExp(`.*${link[1]}`))
                await expect(page.locator('h1', { hasText: new RegExp(`^${link[2]}$`) })).toBeVisible()
            })
        }
    })
})