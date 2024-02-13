import { test, expect } from '@playwright/test'

test.describe('Resources and learning', () => {
    test.beforeEach(async ({ page }) => {
        await page.goto('/resources-learning')
    })
    
    test.describe.skip('Filter accordian @accordian', () => {
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

    

    test('Check that Filters exist' , async ({ page }) => {
        await page.getByRole('button', { name: 'Show all sections', exact: true }).click()
        
        await page.locator('.govuk-checkboxes__label').last().click()

        // Check for the existence of checkboxes with labels
        const checkboxes = await Promise.all([
            page.$('input[id="tag-careerStagePractitioner"]'),
            page.$('input[id="tag-careerStageExperiencedPractitioner"]'),
            page.$('input[id="tag-careerStageManager"]'),
            page.$('input[id="tag-careerStageSeniorManager"]'),
            page.$('input[id="tag-careerStageSeniorLeader"]')
        ]);

        // Log the result
        checkboxes.forEach((checkbox, index) => {
            const label = ["Practitioner", "Experienced Practitioner", "Manager", "Senior Manager", "Senior Leader"][index];
            console.log(`${label} checkbox exists: ${!!checkbox}`);
            expect(checkbox).toBeTruthy();
        });
        
    })


    /////
    test('Check that Filters exist and are in correct order', async ({ page }) => {
        await page.getByRole('button', { name: 'Show all sections', exact: true }).click();
        
        await page.locator('.govuk-checkboxes__label').last().click();
    
        // Check for the existence of checkboxes with labels
        const checkboxes = await Promise.all([
            page.$('input[id="tag-careerStagePractitioner"]'),
            page.$('input[id="tag-careerStageExperiencedPractitioner"]'),
            page.$('input[id="tag-careerStageManager"]'),
            page.$('input[id="tag-careerStageSeniorManager"]'),
            page.$('input[id="tag-careerStageSeniorLeader"]')
        ]);
    
        // Log the result
        for (let i = 0; i < checkboxes.length - 1; i++) {
            const checkbox = checkboxes[i];
            const nextCheckbox = checkboxes[i + 1];
            const label = ["Practitioner", "Experienced Practitioner", "Manager", "Senior Manager", "Senior Leader"][i];
            
            console.log(`${label} checkbox exists: ${!!checkbox}`);
            expect(checkbox).toBeTruthy();
            
            if (checkbox && nextCheckbox) {
                // Evaluate the positions of checkboxes
                const order = await page.evaluate(({ rect1, rect2 }) => {
                    if (rect1.y < rect2.y) {
                        return 'before';
                    } else if (rect1.y > rect2.y) {
                        return 'after';
                    } else {
                        return 'same';
                    }
                }, {
                    rect1: await checkbox.boundingBox(),
                    rect2: await nextCheckbox.boundingBox()
                });
                
                expect(order).toBe('before'); // Assuming checkboxes are vertically aligned
            }
        }
    });
    
    

})