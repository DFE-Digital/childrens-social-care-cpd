import { test, expect } from '@playwright/test'

test.describe('Accessibility', () => {
    test.beforeEach(async ({ page }) => {
        await page.goto('/accessibility')
    })
    
    test('CPD email address is visible @external', async ({ page }) => {
        await expect(page.getByRole('link', { name: 'develop-child-family-social-work-career-team@digital.education.gov.uk' })).toBeVisible()
    })

    test.describe('External links @external', () => {
        const links = [
            ['AbilityNet', 'https://mcmw.abilitynet.org.uk/'],
            ['Web Content Accessibility Guidelines version (WCAG) 2.1', 'https://www.w3.org/TR/WCAG21/'],
            ['Read tips on contacting organisations about inaccessible websites', 'https://www.w3.org/WAI/teach-advocate/contact-inaccessible-websites/'],
            ['contact the Equality Advisory and Support Service (EASS)', 'https://www.equalityadvisoryservice.com/'],
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