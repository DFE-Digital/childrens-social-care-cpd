import { test, expect } from '@playwright/test'

test.describe('Error conditions', () => {

    test('When a non existant page is visited, the 404 page is shown', async ({ page }) => {
        await page.goto('does-not-exist')

        await expect(await page.getByRole('heading', { name: 'Page not found' })).toBeVisible()
    })


    //   test('Accepting cookies allows you to hide the banner', async ({ page }) => {
    //     await page.getByRole('button', { name: 'Accept analytics cookies' }).click()
    //     await page.getByRole('button', { name: 'Hide cookie message' }).click()

    //     await expect(await page.locator('#divCookieBannerId')).not.toBeVisible()
    //   })

    //   test('Rejecting cookies allows you to hide the banner', async ({ page }) => {
    //     await page.getByRole('button', { name: 'Reject analytics cookies' }).click()
    //     await page.getByRole('button', { name: 'Hide cookie message' }).click()

    //     await expect(await page.locator('#divCookieBannerId')).not.toBeVisible()
    //   })

})
