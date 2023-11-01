import { test, expect } from '@playwright/test'

test.describe('Resources and learning', () => {
    test.beforeEach(async ({ page }) => {
        await page.goto('/resources-learning')
    })
    
    test.describe('Filter accordian @accordian', () => {
        test('All sections are collapsed when you arrive on page' , async ({ page }) => {
            const sections = await page.locator('.govuk-accordion__section-content').all()
            expect(sections.length).toBeGreaterThan(1)
            for (const section of sections)
            {
                await expect(section).toHaveAttribute('hidden', 'until-found')
            }
        })

        test('Clicking expand all, expands the sections' , async ({ page }) => {
            await page.getByRole('button', { name: 'Show all sections', exact: true }).click()

            const sections = await page.locator('.govuk-accordion__section-content').all()
            expect(sections.length).toBeGreaterThan(1)
            for (const section of sections)
            {
                await expect(section).not.toHaveAttribute('hidden', 'until-found')
            }
        })
    })

    test('Filter by tags performs search' , async ({ page }) => {
        await page.getByRole('button', { name: 'Show all sections', exact: true }).click()
        const beforeText = await page.getByTestId("results-count").textContent()
        await page.locator('.govuk-checkboxes__label').first().click()
        await page.getByRole('button', { name: 'Apply filter', exact: true }).click()
        const afterText = await page.getByTestId("results-count").textContent()
        expect(beforeText).not.toEqual(afterText)
    })
})