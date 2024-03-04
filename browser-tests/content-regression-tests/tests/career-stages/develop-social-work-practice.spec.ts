import { test, expect } from '@playwright/test'

test.describe('Practitioner', () => {

    test('User journey via menu @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByLabel('Menu').getByRole('link', { name: 'Career stages', exact: true }).click()
        await page.getByRole('link', { name: 'Practitioner', exact: true }).click()
        
        await expect(page.locator('h1', { hasText: /^Practitioner$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/practitioner/)
        await expect(page.locator('#mmi-career')).toHaveClass(/dfe-header__navigation-item--current/)
        
    })

})

test.describe('Experienced Practitioner', () => {

    test('User journey from homepage @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByRole('link', { name: 'Practitioner', exact: true }).click()
        await page.getByRole('link', { name: 'Experienced practitioner', exact: true }).click()

        await expect(page.locator('h1', { hasText: /^Experienced practitioner$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*experienced-practitioner/)
        await expect(page.locator('#mmi-career')).toHaveClass(/dfe-header__navigation-item--current/)
        
    })

})

test.describe('Manager', () => {

    test('User journey via menu @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByLabel('Menu').getByRole('link', { name: 'Career stages', exact: true }).click()
        await page.getByRole('link', { name: 'Manager', exact: true }).click()
        
        await expect(page.locator('h1', { hasText: /^Manager$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/manager/)
        await expect(page.locator('#mmi-career')).toHaveClass(/dfe-header__navigation-item--current/)
        
    })

})

test.describe('Senior Manager', () => {

    test('User journey via menu @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByLabel('Menu').getByRole('link', { name: 'Career stages', exact: true }).click()
        await page.getByRole('link', { name: 'Senior manager', exact: true }).click()
        
        await expect(page.locator('h1', { hasText: /^Senior manager$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/senior-manager/)
        await expect(page.locator('#mmi-career')).toHaveClass(/dfe-header__navigation-item--current/)
        
    })

})

test.describe('Senior Leader', () => {

    test('User journey via menu @journey', async ({ page }) => {
        await page.goto('/')
        await page.getByLabel('Menu').getByRole('link', { name: 'Career stages', exact: true }).click()
        await page.getByRole('link', { name: 'Senior leader', exact: true }).click()
        
        await expect(page.locator('h1', { hasText: /^Senior leader$/ })).toBeVisible()
        await expect(page).toHaveURL(/.*\/senior-leader/)
        await expect(page.locator('#mmi-career')).toHaveClass(/dfe-header__navigation-item--current/)
        
    })

})