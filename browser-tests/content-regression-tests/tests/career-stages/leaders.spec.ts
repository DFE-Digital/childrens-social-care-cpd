import { test, expect } from '@playwright/test'

// /leaders no loger seems to exist

test.describe('Senior leaders', () => {
    test('User journey from homepage @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByRole('link', { name: 'Senior leader', exact: true }).click()

        await expect(page.locator('h1', { hasText: /^Senior leader$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/senior-leader/)
        await expect(page.locator('#mmi-career')).toHaveClass(/dfe-header__navigation-item--current/)
    })

    test('User journey via menu @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByLabel('Menu').getByRole('link', { name: 'Career stages', exact: true }).click()
        await page.getByRole('link', { name: 'Senior leader', exact: true }).click()

        await expect(page.locator('h1', { hasText: /^Senior leader$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/senior-leader/)
        await expect(page.locator('#mmi-career')).toHaveClass(/dfe-header__navigation-item--current/)
    })

    test.describe('Links', () => {
        const links = [
            ['Upon: aspirant directors programme', '/aspirant-directors-programme', 'Upon: aspirant directors programme'],
            ['Upon: new directors programme', '/new-directors-programme', 'Upon: new directors programme'],
            ['Pathway 4: practice leaders', '/pathway-4', 'Pathway 4: practice leaders'],
            ['Assistant director', '/assistant-director', 'Assistant director'],
            ["Director of children's services", '/director-children-services', "Director of children's services"],
            ['Explore roles', '/explore-roles', 'Roles in child and family social work'],
        ]

        for (const link of links) {
            test(`Goes to the ${link[0]} page`, async ({ page }) => {
                await page.goto('/career-stages/senior-leader')
                await page.getByRole('link', { name: link[0], exact: true }).last().click()
                await expect(page).toHaveURL(new RegExp(`.*${link[1]}`))
                await expect(page.locator('h1', { hasText: new RegExp(`^${link[2]}$`) })).toBeVisible()
            })
        }
    })
})