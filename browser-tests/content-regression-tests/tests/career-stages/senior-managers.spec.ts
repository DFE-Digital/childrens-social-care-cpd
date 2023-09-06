import { test, expect } from '@playwright/test'

test.describe('Senior managers', () => {
    test('User journey from homepage @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByRole('link', { name: 'Senior managers', exact: true }).click()

        await expect(page.locator('h1', { hasText: /^Senior managers$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/senior-managers/)
        await expect(page.locator('#mmi-career')).toHaveClass(/dfe-header__navigation-item--current/)
    })

    test('User journey via menu @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByLabel('Menu').getByRole('link', { name: 'Career stages', exact: true }).click()
        await page.getByRole('link', { name: 'Senior managers', exact: true }).click()

        await expect(page.locator('h1', { hasText: /^Senior managers$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/senior-managers/)
        await expect(page.locator('#mmi-career')).toHaveClass(/dfe-header__navigation-item--current/)
    })

    test.describe('Links', () => {
        const links = [
            ['Pathway 3: heads of service', '/pathway-3', 'Pathway 3: heads of service'],
            ['Pathway 4: practice leaders', '/pathway-4', 'Pathway 4: practice leaders'],
            ['View all programmes', '/development-programmes', 'Child and family social work development programmes'],
            ['Explore all roles', '/explore-roles', 'Roles in child and family social work'],
        ]

        for (const link of links) {
            test(`Goes to the ${link[0]} page`, async ({ page }) => {
                await page.goto('/senior-managers')
                await page.getByRole('link', { name: link[0], exact: true }).click()
                await expect(page).toHaveURL(new RegExp(`.*${link[1]}`))
                await expect(page.locator('h1', { hasText: new RegExp(`^${link[2]}$`) })).toBeVisible()
            })
        }
    })
})