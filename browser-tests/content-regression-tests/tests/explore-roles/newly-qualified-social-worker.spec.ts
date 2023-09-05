import { test, expect } from '@playwright/test'

test.describe('Experienced Practitioners', () => {
    test('User journey via Explore menu @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByLabel('Menu').getByRole('link', { name: 'Explore roles', exact: true }).click()
        await page.getByRole('link', { name: 'Newly qualified social worker', exact: true }).click()
        
        await expect(page.locator('h1', { hasText: /^Newly qualified social worker$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/explore-roles\/newly-qualified-social-worker/)
        await expect(page.locator('#mmi-exploreRoles')).toHaveClass(/dfe-header__navigation-item--current/)
    })

    test.describe('Accordian @accordian', () => {
        test('All sections are collapsed when you arrive on page' , async ({ page }) => {
            await page.goto('/explore-roles/newly-qualified-social-worker')
            await expect(page.getByLabel('What you\'ll do , Show this section')).toHaveAttribute('aria-expanded', 'false')
            await expect(page.getByLabel('Skills and knowledge , Show this section')).toHaveAttribute('aria-expanded', 'false')
            await expect(page.getByLabel('How to become one , Show this section')).toHaveAttribute('aria-expanded', 'false')
            await expect(page.getByLabel('Career paths and progression , Show this section')).toHaveAttribute('aria-expanded', 'false')
            await expect(page.getByLabel('Current opportunities , Show this section')).toHaveAttribute('aria-expanded', 'false')
        })

        test('Clicking show all sections expand all sections' , async ({ page }) => {
            await page.goto('/explore-roles/newly-qualified-social-worker')
            await page.getByRole('button', { name: 'Show all sections' }).click()

            await expect(page.getByLabel('What you\'ll do , Hide this section')).toHaveAttribute('aria-expanded', 'true')
            await expect(page.getByLabel('Skills and knowledge , Hide this section')).toHaveAttribute('aria-expanded', 'true')
            await expect(page.getByLabel('How to become one , Hide this section')).toHaveAttribute('aria-expanded', 'true')
            await expect(page.getByLabel('Career paths and progression , Hide this section')).toHaveAttribute('aria-expanded', 'true')
            await expect(page.getByLabel('Current opportunities , Hide this section')).toHaveAttribute('aria-expanded', 'true')
        })

        test('Clicking show all sections, then hide all sections collapses all sections' , async ({ page }) => {
            await page.goto('/explore-roles/newly-qualified-social-worker')
            await page.getByRole('button', { name: 'Show all sections' }).click()
            await page.getByRole('button', { name: 'Hide all sections' }).click()

            await expect(page.getByLabel('What you\'ll do , Show this section')).toHaveAttribute('aria-expanded', 'false')
            await expect(page.getByLabel('Skills and knowledge , Show this section')).toHaveAttribute('aria-expanded', 'false')
            await expect(page.getByLabel('How to become one , Show this section')).toHaveAttribute('aria-expanded', 'false')
            await expect(page.getByLabel('Career paths and progression , Show this section')).toHaveAttribute('aria-expanded', 'false')
            await expect(page.getByLabel('Current opportunities , Show this section')).toHaveAttribute('aria-expanded', 'false')
        })

        test.describe('Sections', () => {
            const links = [
                ['What you\'ll do , Show this section', 'What you\'ll do , Hide this section'],
                ['Skills and knowledge , Show this section', 'Skills and knowledge , Hide this section'],
                ['How to become one , Show this section', 'How to become one , Hide this section'],
                ['Career paths and progression , Show this section', 'Career paths and progression , Hide this section'],
                ['Current opportunities , Show this section', 'Current opportunities , Hide this section'],
            ]

            for (const link of links) {
                test(`Expanding/Collapsing the '${link[0]}'`, async ({ page }) => {
                    await page.goto('/explore-roles/newly-qualified-social-worker')
                    await page.getByLabel(link[0]).click()

                    await expect(page.getByLabel(link[1])).toHaveAttribute('aria-expanded', 'true')

                    await page.getByLabel(link[1]).click()

                    await expect(page.getByLabel(link[0])).toHaveAttribute('aria-expanded', 'false')
                })
            }
        })
    })
})