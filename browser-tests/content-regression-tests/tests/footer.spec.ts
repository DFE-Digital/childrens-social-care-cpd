import { test, expect } from '@playwright/test'

test.beforeEach(async ({ page }) => {
    await page.goto('/')
})

test.describe('Page Footer', () => {
    const links = [
        ['Cookies', '/cookies', 'Cookies'],
        ['Privacy policy', '/privacy', 'Privacy policy'],
        ['Accessibility', '/accessibility', 'Accessibility statement'],
        ['Terms and conditions', '/termsconditions', 'Terms and conditions'],
        ['Sitemap', '/sitemap', 'Sitemap'],
    ]

    for (const link of links) {
        test(`Footer contains link ${link[0]} that goes to ${link[1]}`, async ({ page }) => {
            var promise = page.waitForResponse(`**${link[1]}`)
            await page.getByRole('link', { name: link[0], exact: true }).click()
            var response = await promise

            expect(response.ok()).toBeTruthy()
            await expect(page.locator('h1', { hasText: new RegExp(`^${link[2]}$`) })).toBeVisible()
        })
    }

    test.describe('External links @external', () => {
        const links = [
            ['Feedback', 'https://dferesearch.fra1.qualtrics.com/jfe/form/SV_bmcLDedq5wipeTA'],
            ['Open Government Licence v3.0', 'https://www.nationalarchives.gov.uk/doc/open-government-licence/version/3/'],
            ['Crown copyright', 'https://www.nationalarchives.gov.uk/information-management/re-using-public-sector-information/uk-government-licensing-framework/crown-copyright/'],
        ]
        
        for (const link of links) {
            test(`'${link[0]}' goes to ${link[1]}`, async ({ page }) => {
                var promise = page.waitForResponse(`**${link[1]}`)
                await page.getByRole('link', { name: link[0], exact: true }).click()
                var response = await promise
                
                expect(response.ok()).toBeTruthy()
            })
        }
    })
})