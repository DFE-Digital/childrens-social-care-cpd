
import { test, expect } from '@playwright/test';

test.describe('Details', () => {

    let elements = {};

    test.beforeEach(async ({ page }) => {
        await page.goto('/demo/details');

        elements['container'] = page.getByTestId('details-container');
        elements['summary-container'] = page.getByTestId('details-summary');
        elements['summary-link'] = elements['summary-container'].locator('span');
        elements['text-container'] = page.getByTestId('details-text');
    });

    test('Padding, margins, appearance', async ({ page }) => {
        await expect(elements['container']).toHaveCSS('margin-bottom', '30px');
        await expect(elements['summary-container']).toHaveCSS('padding-left', '25px');
        await expect(elements['summary-link']).toHaveCSS('cursor', 'pointer');
        await expect(elements['summary-link']).toHaveCSS('text-decoration', 'underline 1px solid rgb(29, 112, 184)');
        await expect(elements['summary-link']).toHaveCSS('color', 'rgb(29, 112, 184)');
        await expect(elements['text-container']).toHaveCSS('border-left', '5px solid rgb(177, 180, 182)');
        await expect(elements['text-container']).toHaveCSS('padding-bottom', '15px');
        await expect(elements['text-container']).toHaveCSS('padding-left', '20px');
        await expect(elements['text-container']).toHaveCSS('padding-top', '15px');
    });

    test('Show/hide on click', async ({ page }) => {
        await expect(elements['text-container']).not.toBeVisible();
        await elements['summary-link'].click();
        await expect(elements['text-container']).toBeVisible();
        await elements['summary-link'].click();
        await expect(elements['text-container']).not.toBeVisible();
    });

});

