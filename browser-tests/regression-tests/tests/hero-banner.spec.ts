
import { test, expect } from '@playwright/test';

test.describe('Hero Banner', () => {
    test.beforeEach(async ({ page }) => {
        await page.goto('/demo/hero-banner/example');
    });

    test.describe('Title', () => {
        test('Has correct styles', async ({ page }) => {
            const title = page.getByTestId('hero-banner-title');
            await expect(title).toHaveCSS('margin-bottom', '30px');
            await expect(title).toHaveCSS('margin-top', '0px');
            await expect(title).toHaveCSS('font-weight', '700');
            await expect(title).toHaveCSS('font-size', '48px');
            await expect(title).toHaveCSS('line-height', '63.9998px');
        });
    });

    test.describe('Text', () => {
        test('Has correct styles', async ({ page }) => {
            const text = page.getByTestId('hero-banner-text');
            await expect(text).toHaveCSS('margin-bottom', '20px');
            await expect(text).toHaveCSS('margin-top', '0px');
            await expect(text).toHaveCSS('font-weight', '400');
            await expect(text).toHaveCSS('font-size', '24px');
            await expect(text).toHaveCSS('line-height', '30px');
        });
    });

    test.describe('Container', () => {
        test('Has correct styles', async ({ page }) => {
            const container = page.getByTestId('hero-banner-container');
            await expect(container).toHaveCSS('margin-bottom', '50px');
            await expect(container).toHaveCSS('padding-top', '40px');
            await expect(container).toHaveCSS('padding-bottom', '35px');
            await expect(container).toHaveCSS('background-color', 'rgb(52, 124, 169)');
        });
    });

    
});

