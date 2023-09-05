import { test, expect } from '@playwright/test'

test.describe('Assessed and supported year in employment (ASYE)', () => {
    test('User journey from homepage @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByRole('link', { name: 'Practitioners', exact: true }).click()
        await page.getByRole('link', { name: 'Assessed and supported year in employment (ASYE)', exact: true }).click()

        await expect(page.locator('h1', { hasText: /^Assessed and supported year in employment \(ASYE\)$/ })).toBeVisible()
        await expect(page.locator('#mmi-developmentProgrammes')).toHaveClass(/dfe-header__navigation-item--current/)
    })

    test('User journey via Career menu @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByLabel('Menu').getByRole('link', { name: 'Career information', exact: true }).click()
        await page.getByRole('link', { name: 'Practitioners', exact: true }).click()
        await page.getByRole('link', { name: 'Assessed and supported year in employment (ASYE)', exact: true }).click()

        await expect(page.locator('h1', { hasText: /^Assessed and supported year in employment \(ASYE\)$/ })).toBeVisible()
        await expect(page.locator('#mmi-developmentProgrammes')).toHaveClass(/dfe-header__navigation-item--current/)
    })

    test('User journey via Development menu @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByLabel('Menu').getByRole('link', { name: 'Development programmes', exact: true }).click()
        await page.getByRole('link', { name: 'Assessed and supported year in employment (ASYE)', exact: true }).click()

        await expect(page.locator('h1', { hasText: /^Assessed and supported year in employment \(ASYE\)$/ })).toBeVisible()
        await expect(page.locator('#mmi-developmentProgrammes')).toHaveClass(/dfe-header__navigation-item--current/)
    })

    test.describe('External links @external', () => {
        const links = [
            ['Skills for Care', 'https://www.skillsforcare.org.uk/Regulated-professions/Social-work/ASYE/ASYE.aspx'],
            ['standards that enable social workers to work effectively and safely', 'https://www.local.gov.uk/our-support/workforce-and-hr-support/social-workers/standards-employers-social-workers-england-2020'],
            ['post qualifying standards', 'https://www.gov.uk/government/publications/knowledge-and-skills-statements-for-child-and-family-social-work'],
            ['Professional Capabilities Framework', 'https://www.basw.co.uk/social-work-training/professional-capabilities-framework-pcf'],
            ['Find out more about each ASYE stage', 'https://www.skillsforcare.org.uk/Regulated-professions/Social-work/ASYE/ASYE-templates.aspx'],
            ['Find out more about the ASYE', 'https://www.skillsforcare.org.uk/Regulated-professions/Social-work/ASYE/ASYE.aspx'],
            ['Social work post-qualifying standards', 'https://www.gov.uk/government/publications/knowledge-and-skills-statements-for-child-and-family-social-work'],
        ]
        
        for (const link of links) {
            test(`'${link[0]}' goes to ${link[1]}`, async ({ page }) => {
                await page.goto('/asye')
                var promise = page.waitForResponse(`**${link[1]}`)
                await page.getByRole('link', { name: link[0], exact: true }).click()
                var response = await promise
                
                expect(response.ok()).toBeTruthy()
            })
        }
    })
})