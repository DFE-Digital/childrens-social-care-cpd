import { test, expect } from '@playwright/test'

test.describe('Page banner', () => {
    
    test.describe('When enabled', () => {
        test.beforeEach(async ({ page }) => {
            await page.goto('dev-integration')
        })
    
        test('Title is visible', async ({ page }) => {
            const title = await page.locator('#content-banner-title')
            await expect(title).toBeVisible()
            await expect(title).toHaveClass(/govuk-heading-xl/)
        })
    
        test('Subtitle is visible', async ({ page }) => {
            const subtitle = await page.locator('#content-banner-subtitle')
            await expect(subtitle).toBeVisible()
            await expect(subtitle).toHaveClass(/govuk-body-l/)
        })
    })

    test.describe('When not enabled', () => {
        test('Is not visible', async ({ page }) => {
            await page.goto('/')
            await expect(page.locator('#content-banner')).not.toBeAttached()
        })
    })
})