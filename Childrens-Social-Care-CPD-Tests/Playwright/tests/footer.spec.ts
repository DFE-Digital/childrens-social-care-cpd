import { test, expect } from '@playwright/test'

test.beforeEach(async ({ page }) => {
    await page.goto('content')
})

test.describe('Page Footer', () => {

    const links = [
        ['Privacy policy', '/content/privacy'],
        //['Cookies', '/cookies'],
        ['Accessibility', '/content/accessibility'],
        ['Terms and conditions', '/content/termsconditions'],
        ['Feedback', 'https://dferesearch.fra1.qualtrics.com/jfe/form/SV_bmcLDedq5wipeTA'],
        ['Crown copyright', 'https://www.nationalarchives.gov.uk/information-management/re-using-public-sector-information/uk-government-licensing-framework/crown-copyright/'],
    ]

    for (const link of links) {
        test(`Footer contains link to ${link[0]} at ${link[1]}`, async ({ page }) => {
            const l = await page.getByRole('link', { name: link[0], exact: true })
            
            await expect(l).toBeVisible()
            await expect(await l.getAttribute('href')).toBe(link[1])
        })
    }

    test(`Footer contains link to the Cookies page`, async ({ page }) => {
        const l = await page.getByRole('link', { name: 'Cookies', exact: true })
        
        await expect(l).toBeVisible()
        const href = await l.getAttribute('href')

        await expect(href?.startsWith('/cookies?')).toBeTruthy()
    })

})