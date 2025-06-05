
import { test, expect } from '@playwright/test';

test.describe('Navigation Menu with Header', () => {
    let elements = {};

    test.beforeEach(async ({ page }) => {
        await page.goto('/demo/navigation-menu');

        elements['container'] = page.getByTestId('navigation-menu-container');
        elements['header'] = elements['container'].locator('h2');
        elements['nav-container'] = elements['container'].locator('nav');
        elements['nav-link-containers'] = elements['container'].locator('li');
        elements['inactive-nav-element-container'] = elements['nav-link-containers'].locator('nth=0');
        elements['inactive-nav-element'] = elements['inactive-nav-element-container'].locator('a');
        elements['active-nav-element-container'] = elements['nav-link-containers'].locator('nth=2');
        elements['active-nav-element'] = elements['active-nav-element-container'].locator('a');
    });

    test('Containers', async ({ page }) => {

        // the container for the whole nav
        await expect(elements['container']).toHaveCSS('padding-left', '15px');
        await expect(elements['container']).toHaveCSS('padding-right', '15px');

        // the nav header
        await expect(elements['header']).toHaveCSS('margin-bottom', '24px');
        await expect(elements['header']).toHaveCSS('font-weight', '700');
        await expect(elements['header']).toHaveCSS('font-size', '24px');

        // the container for the collection of nav itmes
        await expect(elements['nav-container']).toHaveCSS('margin-left', '-15px');
        await expect(elements['nav-container']).toHaveCSS('padding-left', '15px');
    });

    test('Nav items', async ({ page }) => {

        await expect(elements['nav-link-containers']).toHaveCount(16);

        // an inactive (not current page) nav item
        await expect(elements['inactive-nav-element-container']).toHaveCSS('border-left', '4px solid rgb(177, 180, 182)');
        await expect(elements['inactive-nav-element-container']).toHaveCSS('margin-bottom', '8px');
        await expect(elements['inactive-nav-element']).toHaveCSS('padding', '7px 30px 8px 10px');
        await expect(elements['inactive-nav-element']).toHaveCSS('margin-bottom', '5px');
        await expect(elements['inactive-nav-element']).toHaveCSS('cursor', 'pointer');
        await expect(elements['inactive-nav-element']).toHaveCSS('font-weight', '400');

        // hover styles for an inactive nav item
        await elements['inactive-nav-element-container'].hover();
        await expect(elements['inactive-nav-element-container']).toHaveCSS('border-left', '4px solid rgb(52, 124, 169)');
    });

    test('Active nav item', async ({ page }) => {

        // an active (current page) nav item
        await expect(elements['active-nav-element-container']).toHaveCSS('border-left', '4px solid rgb(0, 58, 105)');
        await expect(elements['active-nav-element-container']).toHaveCSS('margin-bottom', '8px');
        await expect(elements['active-nav-element-container']).toHaveCSS('background-color', 'rgb(243, 242, 241)');
        await expect(elements['active-nav-element']).toHaveCSS('padding', '7px 30px 8px 10px');
        await expect(elements['active-nav-element']).toHaveCSS('margin-bottom', '5px');
        await expect(elements['active-nav-element']).toHaveCSS('cursor', 'pointer');
        await expect(elements['active-nav-element']).toHaveCSS('font-weight', '700');
        await expect(elements['active-nav-element']).toHaveCSS('color', 'rgb(0, 58, 105)');

        // hover styles for an active nav item
        await elements['active-nav-element-container'].hover();
        await expect(elements['active-nav-element-container']).toHaveCSS('border-left', '4px solid rgb(52, 124, 169)');
    });

});

test.describe('Navigation Menu without Header', () => {
    let elements = {};
    test.beforeEach(async ({ page }) => {
        await page.goto('/demo/navigation-menu/no-header');
        elements['container'] = page.getByTestId('navigation-menu-container');
        elements['nav-link-containers'] = elements['container'].locator('li');
    });
});

