import { test, expect } from '@playwright/test'

test.describe('Error conditions', () => {

    test('When a non existant page is visited, the 404 page is shown', async ({ page }) => {
        await page.goto('does-not-exist')

        await expect(await page.getByRole('heading', { name: 'Page not found' })).toBeVisible()
    })

})
