import { test, expect } from '@playwright/test'

test.describe('Error conditions', () => {


    
    test('When a non existant page is visited, the 404 page is shown', async ({ page }) => {
        await page.goto('does-not-exist')

        await expect(await page.getByRole('heading', { name: 'Page not found' })).toBeVisible()
    })

    test('When a non existant page is visited, the page is not redirected to an error URL', async ({ page }) => {
        await page.goto('does-not-exist')

        await expect(page).toHaveURL(/\/does-not-exist/)
    })

    test('Error page header nav is synced with main site header', async ({ page }) => {

        // get everything we need from the home page first
        await page.goto('content')
        
        const mainNavs = await page.locator("[id^=mmi-]")
        const mainNavCount = await mainNavs.count();
        const mainPageNavs = new Map()

        for (let index = 0; index < mainNavCount; index++)
        {
            const nav = await mainNavs.nth(index)
            const id = await nav.getAttribute("id")
            const anchor = nav.locator("a")
            const text = await anchor.innerText()
            const href = await anchor.getAttribute("href")
            
            mainPageNavs.set(id, { text, href })
        }

        // now we can compare to the error page
        await page.goto('does-not-exist')
        const errorNavs = await page.locator("[id^=mmi-]")
        const errorNavCount = await errorNavs.count()

        expect(mainNavCount).toEqual(errorNavCount)

        for (const [key, value] of mainPageNavs)
        {
            const anchor = await page.locator(`[id=${key}] a`)

            await expect(anchor).toBeAttached();
            await expect(anchor).toHaveAttribute('href', value.href)
            await expect(anchor).toHaveText(value.text)
        }
    })

})
