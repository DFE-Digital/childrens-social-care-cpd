import { test, expect } from '@playwright/test'

test.describe('Homepage', () => {
    test.beforeEach(async ({ page }) => {
        await page.goto('/')
    })

    test.describe('CTA links', () => {
        test.describe('Explore career stages', () => {
            const links = [
                ['Practitioner', '/practitioner', 'Practitioners'],
                ['Experienced practitioner', '/experienced-practitioner', 'Experienced practitioners'],
                ['Manager', '/manager', 'Managers'],
                ['Senior manager', '/senior-manager', 'Senior managers'],
                ['Leader', '/leader', 'Leaders'],
            ]

            for (const link of links) {
                test(`Goes to the ${link[0]} page`, async ({ page }) => {
                    var responsePromise = page.waitForResponse(`**${link[1]}`)
                    await page.getByRole('link', { name: link[0], exact: true }).click()
                    var response = await responsePromise
                    
                    expect(response.ok()).toBeTruthy()

                    await expect(page.locator('h1', { hasText: new RegExp(`^${link[2]}$`) })).toBeVisible()
                })
            }
        })

        test.describe('Useful information', () => {
            const links = [
                ['DfE funded programmes', '/development-programmes', 'Child and family social work development programmes'],
                ['Explore roles', '/explore-roles', 'Roles in child and family social work'],
            ]

            for (const link of links) {
                test(`Goes to the ${link[0]} page`, async ({ page }) => {
                    var responsePromise = page.waitForResponse(`**${link[1]}`)
                    await page.getByRole('link', { name: link[0], exact: true }).click()
                    var response = await responsePromise
                    
                    expect(response.ok()).toBeTruthy()

                    await expect(page.locator('h1', { hasText: new RegExp(`^${link[2]}$`) })).toBeVisible()
                })
            }
        })
    })

    
})