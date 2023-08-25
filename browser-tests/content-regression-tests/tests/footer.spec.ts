import { test, expect } from '@playwright/test'

test.beforeEach(async ({ page }) => {
    await page.goto('/')
})

test.describe('Page Footer', () => {

    const links = [
        ['Cookies', '/cookies'],
        ['Privacy policy', '/privacy'],
        ['Accessibility', '/accessibility'],
        ['Terms and conditions', '/termsconditions'],
        ['Feedback', 'https://dferesearch.fra1.qualtrics.com/jfe/form/SV_bmcLDedq5wipeTA'],
        ['Crown copyright', 'https://www.nationalarchives.gov.uk/information-management/re-using-public-sector-information/uk-government-licensing-framework/crown-copyright/'],
        ['Open Government Licence v3.0', 'https://www.nationalarchives.gov.uk/doc/open-government-licence/version/3/'],
    ]

    for (const link of links) {
        test(`Footer contains link ${link[0]} that goes to ${link[1]}`, async ({ page }) => {
            var responsePromise = page.waitForResponse(`**${link[1]}`)
            await page.getByRole('link', { name: link[0], exact: true }).click()
            var response = await responsePromise

            expect(response.ok()).toBeTruthy()
        })
    }

})