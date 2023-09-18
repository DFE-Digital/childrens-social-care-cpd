import { test, expect } from '@playwright/test'

test.describe('Experienced Practitioners', () => {

    test('User journey from homepage @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByRole('link', { name: 'Experienced practitioners', exact: true }).click()

        await expect(page.locator('h1', { hasText: /^Experienced practitioners$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/experienced-practitioners/)
        await expect(page.locator('#mmi-career')).toHaveClass(/dfe-header__navigation-item--current/)
    })

    test('User journey via menu @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByLabel('Menu').getByRole('link', { name: 'Career stages', exact: true }).click()
        await page.getByRole('link', { name: 'Experienced practitioners', exact: true }).click()

        await expect(page.locator('h1', { hasText: /^Experienced practitioners$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/experienced-practitioners/)
        await expect(page.locator('#mmi-career')).toHaveClass(/dfe-header__navigation-item--current/)
    })

    test.describe('Links', () => {
        const links = [
            ['Pathway 1: practice supervisors', '/pathway-1', 'Pathway 1: practice supervisors'],
            ['Pathway 2: middle managers', '/pathway-2', 'Pathway 2: middle managers'],
            ['View all programmes', '/development-programmes', 'Child and family social work development programmes'],
            ['Explore all roles', '/explore-roles', 'Explore roles in child and family social work'],
        ]

        for (const link of links) {
            test(`Goes to the ${link[0]} page`, async ({ page }) => {
                await page.goto('/experienced-practitioners')
                await page.getByRole('link', { name: link[0], exact: true }).click()
                await expect(page).toHaveURL(new RegExp(`.*${link[1]}`))
                await expect(page.locator('h1', { hasText: new RegExp(`^${link[2]}$`) })).toBeVisible()
            })
        }
    })
})