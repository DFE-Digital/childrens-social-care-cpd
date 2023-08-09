import { test, expect } from '@playwright/test'

test.describe('Check heading tag CSS', () => {

    test.describe('Homepage', () => {

        test.beforeEach(async ({ page }) => {
            await page.goto('content')
        })

        test('Hero banner h1 tag is extra large', async ({ page }) => {
            await expect(await page.getByTestId('hero-banner-title')).toHaveClass(/govuk-heading-xl/)
        })

        test('h2 tags are medium', async ({ page }) => {
            const h2s = await page.locator('h2:visible')
            const count = await h2s.count()
            for (let i = 0; i < count; i++)
            {
                const item = h2s.nth(i)
                await expect(item).toHaveClass(/govuk-heading-m/)    
            }
        })

        test('h3 tags are medium', async ({ page }) => {
            const h3s = await page.locator('h3:visible')
            const count = await h3s.count()
            for (let i = 0; i < count; i++)
            {
                const item = h3s.nth(i)
                await expect(item).toHaveClass(/govuk-heading-m/)
            }
        })

    })

})