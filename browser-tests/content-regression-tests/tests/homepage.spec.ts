import { test, expect } from '@playwright/test'

test.describe('Homepage', () => {
    test.beforeEach(async ({ page }) => {
        await page.goto('/')
    })

    test.describe('CTA links', () => {
        test.describe('Explore career stages', () => {
            const links = [
                ['Practitioner', '/practitioner', 'Practitioner'],
                ['Experienced practitioner', '/experienced-practitioner', 'Experienced practitioner'],
                ['Manager', '/manager', 'Manager'],
                ['Senior manager', '/senior-manager', 'Senior manager'],
                ['Senior leader', '/senior-leader', 'Senior leader'],
            ]

            for (const link of links) {
                test(`Goes to the ${link[0]} page`, async ({ page }) => {
                    await page.getByRole('link', { name: link[0], exact: true }).click()
                    await expect(page.locator('h1', { hasText: new RegExp(`^${link[2]}$`) })).toBeVisible()
                })
            }
        })

        test.describe('Useful information', () => {
            const links = [
                ['DfE-funded programmes', '/development-programmes', 'Child and family social work development programmes'],
                ['Explore roles', '/explore-roles', 'Roles in child and family social work'],
            ]

            for (const link of links) {
                test(`Goes to the ${link[0]} page`, async ({ page }) => {
                    await page.getByRole('link', { name: link[0], exact: true }).last().click()
                    await expect(page.locator('h1', { hasText: new RegExp(`^${link[2]}$`) })).toBeVisible()
                })
            }
        })
    })

    
})