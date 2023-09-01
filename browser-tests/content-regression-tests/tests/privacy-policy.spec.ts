import { test, expect } from '@playwright/test'

test.describe('Privacy policy', () => {
    test.beforeEach(async ({ page }) => {
        await page.goto('/privacy')
    })
    
    test('ICO email address is visible @external', async ({ page }) => {
        await expect(page.getByRole('link', { name: 'icocasework@ico.org.uk' })).toBeVisible()
    })

    test.describe('External links @external', () => {
        const links = [
            ['adequacy decisions', 'https://commission.europa.eu/law/law-topic/data-protection/international-dimension-data-protection/adequacy-decisions_en'],
            ['Find out more information about your rights', 'https://ico.org.uk/for-the-public/'],
            ['find more information about how the DfE handles personal information in the personal information charter', 'https://www.gov.uk/government/organisations/department-for-education/about/personal-information-charter'],
            ['secure DfE contact form', 'https://form.education.gov.uk/service/Contact_the_Department_for_Education'],
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