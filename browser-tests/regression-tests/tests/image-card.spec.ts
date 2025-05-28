
import { test, expect } from '@playwright/test';

test.describe('Image Card', () => {
    test.beforeEach(async ({ page }) => {
        await page.goto('/demo/image-card');
    });

    test('Image Card with no text', async ({ page }) => {
        const sectionElement = page.getByTestId('image-card').locator('nth=2');
        await expect(sectionElement.locator('div.dfe-o-banner__content-container')).toHaveCount(0);

        const imageElement = sectionElement.locator('img');
        await expect(imageElement).toBeVisible();
        await expect(imageElement).toHaveAttribute('src', '//images.ctfassets.net/ltt4ovp0xx0f/5Kurh5w5f1cCRNf9gsILnR/ab3a8c14083a840c2828a91a1d4f9866/4Cs-Diagram---All-4-Cs.png');
    });

    test('Image Card with left-positioned text', async ({ page }) => {
        const textElement = page.getByText('left-positioned');
        await expect(textElement).toHaveCSS('margin-bottom', '20px');
        
        const sectionElement = page.getByTestId('image-card').filter({ has: textElement});
        await expect(sectionElement).toHaveCSS('margin-bottom', '30px');
        await expect(sectionElement.locator('div.dfe-o-banner__content-container')).toHaveCount(1);

        const containerElement = sectionElement.locator('div.dfe-o-banner');
        await expect(containerElement).toHaveCSS('margin-top', '10px');

        const imageElement = page.locator('img:right-of(:text("left-positioned"))');
        await expect(imageElement).toBeVisible();
        await expect(imageElement).toHaveAttribute('src', '//images.ctfassets.net/ltt4ovp0xx0f/76OfDltxPI65epOQjChzWM/a44b1a1c559de7679a43a49916d21d6b/A_framework_for_embedding_successful_digital_practice.png');
    });

    test('Image Card with right-positioned text', async ({ page }) => {
        const textElement = page.getByText('right-positioned');
        await expect(textElement).toHaveCSS('margin-bottom', '20px');
        
        const sectionElement = page.getByTestId('image-card').filter({ has: textElement});
        await expect(sectionElement).toHaveCSS('margin-bottom', '30px');
        await expect(sectionElement.locator('div.dfe-o-banner__content-container')).toHaveCount(1);

        const containerElement = sectionElement.locator('div.dfe-o-banner');
        await expect(containerElement).toHaveCSS('margin-top', '10px');

        const imageElement = page.locator('img:left-of(:text("right-positioned"))').first();
        await expect(imageElement).toBeVisible();
        await expect(imageElement).toHaveAttribute('src', '//images.ctfassets.net/ltt4ovp0xx0f/6YnUgWjoR4oelhwhegcAsP/e4d717f00f45f59bb15bb23c17dcedd7/Employer-Standards_Fishbone-diagram.png');
    });
});

