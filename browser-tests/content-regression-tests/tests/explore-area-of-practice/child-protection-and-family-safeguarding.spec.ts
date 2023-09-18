import { test, expect } from '@playwright/test'

test.describe.skip('Child Protection & Family Safeguarding', () => {
    test('User journey via Explore menu @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByLabel('Menu').getByRole('link', { name: 'Explore roles', exact: true }).click()
        await page.getByRole('link', { name: 'Explore areas of practice', exact: true }).click()
        await page.getByRole('link', { name: 'Child Protection & Family Safeguarding', exact: true }).click()
        
        await expect(page.locator('h1', { hasText: /^Child Protection & Family Safeguarding$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/explore-areas-of-practice\/child-protection-and-safegaurding/)
        await expect(page.locator('#mmi-exploreRoles')).toHaveClass(/dfe-header__navigation-item--current/)
    })

    test.describe('Accordian @accordian', () => {
        test('All sections are collapsed when you arrive on page' , async ({ page }) => {
            await page.goto('/explore-areas-of-practice/child-protection-and-safegaurding')
            await expect(page.getByLabel('What you\'ll do , Show this section')).toHaveAttribute('aria-expanded', 'false')
            await expect(page.getByLabel('Skills and knowledge , Show this section')).toHaveAttribute('aria-expanded', 'false')
            await expect(page.getByLabel('Who you\'ll work with , Show this section')).toHaveAttribute('aria-expanded', 'false')
            await expect(page.getByLabel('How you\'ll work , Show this section')).toHaveAttribute('aria-expanded', 'false')
            await expect(page.getByLabel('Current opportunities , Show this section')).toHaveAttribute('aria-expanded', 'false')
        })

        test('Clicking show all sections expand all sections' , async ({ page }) => {
            await page.goto('/explore-areas-of-practice/child-protection-and-safegaurding')
            await page.getByRole('button', { name: 'Show all sections' }).click()

            await expect(page.getByLabel('What you\'ll do , Hide this section')).toHaveAttribute('aria-expanded', 'true')
            await expect(page.getByLabel('Skills and knowledge , Hide this section')).toHaveAttribute('aria-expanded', 'true')
            await expect(page.getByLabel('Who you\'ll work with , Hide this section')).toHaveAttribute('aria-expanded', 'true')
            await expect(page.getByLabel('How you\'ll work , Hide this section')).toHaveAttribute('aria-expanded', 'true')
            await expect(page.getByLabel('Current opportunities , Hide this section')).toHaveAttribute('aria-expanded', 'true')
        })

        test('Clicking show all sections, then hide all sections collapses all sections' , async ({ page }) => {
            await page.goto('/explore-areas-of-practice/child-protection-and-safegaurding')
            await page.getByRole('button', { name: 'Show all sections' }).click()
            await page.getByRole('button', { name: 'Hide all sections' }).click()

            await expect(page.getByLabel('What you\'ll do , Show this section')).toHaveAttribute('aria-expanded', 'false')
            await expect(page.getByLabel('Skills and knowledge , Show this section')).toHaveAttribute('aria-expanded', 'false')
            await expect(page.getByLabel('Who you\'ll work with , Show this section')).toHaveAttribute('aria-expanded', 'false')
            await expect(page.getByLabel('How you\'ll work , Show this section')).toHaveAttribute('aria-expanded', 'false')
            await expect(page.getByLabel('Current opportunities , Show this section')).toHaveAttribute('aria-expanded', 'false')
        })

        test.describe('Sections', () => {
            const links = [
                ['What you\'ll do , Show this section', 'What you\'ll do , Hide this section'],
                ['Skills and knowledge , Show this section', 'Skills and knowledge , Hide this section'],
                ['Who you\'ll work with , Show this section', 'Who you\'ll work with , Hide this section'],
                ['How you\'ll work , Show this section', 'How you\'ll work , Hide this section'],
                ['Current opportunities , Show this section', 'Current opportunities , Hide this section'],
            ]

            for (const link of links) {
                test(`Expanding/Collapsing the '${link[0]}'`, async ({ page }) => {
                    await page.goto('/explore-areas-of-practice/child-protection-and-safegaurding')
                    await page.getByLabel(link[0]).click()

                    await expect(page.getByLabel(link[1])).toHaveAttribute('aria-expanded', 'true')

                    await page.getByLabel(link[1]).click()

                    await expect(page.getByLabel(link[0])).toHaveAttribute('aria-expanded', 'false')
                })
            }
        })
    })
})