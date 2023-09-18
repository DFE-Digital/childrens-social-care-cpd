import { test, expect } from '@playwright/test'

test.describe('Managers', () => {
    test('User journey from homepage @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByRole('link', { name: 'Managers', exact: true }).click()

        await expect(page.locator('h1', { hasText: /^Managers$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/managers/)
        await expect(page.locator('#mmi-career')).toHaveClass(/dfe-header__navigation-item--current/)
    })

    test('User journey via menu @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByLabel('Menu').getByRole('link', { name: 'Career stages', exact: true }).click()
        await page.getByRole('link', { name: 'Managers', exact: true }).click()

        await expect(page.locator('h1', { hasText: /^Managers$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/managers/)
        await expect(page.locator('#mmi-career')).toHaveClass(/dfe-header__navigation-item--current/)
    })

    test.describe('Links', () => {
        const links = [
            ['Pathway 2: middle managers', '/pathway-2', 'Pathway 2: middle managers'],
            ['Pathway 3: heads of service', '/pathway-3', 'Pathway 3: heads of service'],
            ['View all programmes', '/development-programmes', 'Child and family social work development programmes'],
            ['Explore all roles', '/explore-roles', 'Explore roles in child and family social work'],
        ]

        for (const link of links) {
            test(`Goes to the ${link[0]} page`, async ({ page }) => {
                await page.goto('/managers')
                await page.getByRole('link', { name: link[0], exact: true }).click()
                await expect(page).toHaveURL(new RegExp(`.*${link[1]}`))
                await expect(page.locator('h1', { hasText: new RegExp(`^${link[2]}$`) })).toBeVisible()
            })
        }
    })
})