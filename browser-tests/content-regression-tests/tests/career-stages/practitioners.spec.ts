import { test, expect } from '@playwright/test'

test.describe('Practitioners', () => {
    test('User journey from homepage @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByRole('link', { name: 'Practitioner', exact: true }).click()

        await expect(page.locator('h1', { hasText: /^Practitioner$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/practitioner/)
        await expect(page.locator('#mmi-career')).toHaveClass(/dfe-header__navigation-item--current/)
    })

    test('User journey via menu @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByLabel('Menu').getByRole('link', { name: 'Career stages', exact: true }).click()
        await page.getByRole('link', { name: 'Practitioner', exact: true }).click()

        await expect(page.locator('h1', { hasText: /^Practitioner$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/practitioner/)
        await expect(page.locator('#mmi-career')).toHaveClass(/dfe-header__navigation-item--current/)
    })

    test.describe('Links', () => {
        const links = [
            ['Assessed and supported year in employment (ASYE)', '/asye', 'Assessed and supported year in employment \\(ASYE\\)'],
            ['Develop your social work practice', '/develop-social-work-practice', 'Develop your social work practice'],
            ['Newly qualified social worker', '/explore-roles/newly-qualified-social-worker', 'Newly qualified social worker'],
            ['Social worker', '/explore-roles/social-worker', 'Social worker'],
            ['Explore roles', '/explore-roles', 'Roles in child and family social work'],
        ]

        for (const link of links) {
            test(`Goes to the ${link[0]} page`, async ({ page }) => {
                await page.goto('/career-stages/practitioner')
                await page.getByRole('link', { name: link[0], exact: true }).last().click()
                await expect(page).toHaveURL(new RegExp(`.*${link[1]}`))
                await expect(page.locator('h1', { hasText: new RegExp(`^${link[2]}$`) })).toBeVisible()
            })
        }
    })
})