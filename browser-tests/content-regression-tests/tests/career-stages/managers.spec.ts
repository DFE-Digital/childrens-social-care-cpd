import { test, expect } from '@playwright/test'

test.describe('Managers', () => {
    test('User journey from homepage @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByRole('link', { name: 'Manager', exact: true }).click()

        await expect(page.locator('h1', { hasText: /^Manager$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/manager/)
        await expect(page.locator('#mmi-career')).toHaveClass(/dfe-header__navigation-item--current/)
    })

    test('User journey via menu @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByLabel('Menu').getByRole('link', { name: 'Career stages', exact: true }).click()
        await page.getByRole('link', { name: 'Manager', exact: true }).click()

        await expect(page.locator('h1', { hasText: /^Manager$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/manager/)
        await expect(page.locator('#mmi-career')).toHaveClass(/dfe-header__navigation-item--current/)
    })

    test.describe('Links', () => {
        const links = [
            ['Pathway 2: middle managers', '/pathway-2', 'Pathway 2: middle managers'],
            ['Pathway 3: heads of service', '/pathway-3', 'Pathway 3: heads of service'],
            ['Practice development manager', '/explore-roles/practice-development-manager', 'Practice development manager'],
            ['Quality improvement manager', '/explore-roles/quality-improvement-manager', 'Quality improvement manager'],
            ['Team manager', '/explore-roles/team-manager', 'Team manager'],
            ['Independent reviewing officer', '/explore-roles/independent-reviewing-officer', 'Independent reviewing officer'],
            ['Explore roles', '/explore-roles', 'Roles in child and family social work'],
        ]

        for (const link of links) {
            test(`Goes to the ${link[0]} page`, async ({ page }) => {
                await page.goto('/career-stages/manager')
                await page.getByRole('link', { name: link[0], exact: true }).last().click()
                await expect(page).toHaveURL(new RegExp(`.*${link[1]}`))
                await expect(page.locator('h1', { hasText: new RegExp(`^${link[2]}$`) })).toBeVisible()
            })
        }
    })
})