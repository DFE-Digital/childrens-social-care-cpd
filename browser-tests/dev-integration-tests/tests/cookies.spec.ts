import { test, expect } from '@playwright/test'

const CookieName = 'cookie_consent'
const AcceptValue = 'accept'
const RejectValue = 'reject'

test.describe('Cookies @cookie', () => {

    test.describe('Banner', () => {

        test.beforeEach(async ({ page }) => {
            await page.goto('content')
        })

        test.describe('Interactions', () => {

            test('Homepage shows the cookie banner', async ({ page }) => {
                await expect(await page.locator('#divCookieBannerId')).toBeVisible()
            })
        
            /*
            Given I click the Accept button on the cookies banner
            When I click the Hide button on the cookie banner
            Then the cookie banner is hidden
            */
            test('Accepting cookies allows you to hide the banner', async ({ page }) => {
                await page.getByRole('button', { name: 'Accept analytics cookies' }).click()
                await page.getByRole('button', { name: 'Hide cookie message' }).click()
        
                await expect(await page.locator('#divCookieBannerId')).not.toBeVisible()
            })
        
            test('Rejecting cookies allows you to hide the banner', async ({ page }) => {
                await page.getByRole('button', { name: 'Reject analytics cookies' }).click()
                await page.getByRole('button', { name: 'Hide cookie message' }).click()
        
                await expect(await page.locator('#divCookieBannerId')).not.toBeVisible()
            })
    
            test('The banner cookies link takes you to the cookies page', async ({ page }) => {
                await page.getByRole('link', { name: 'View cookies' }).click()
                
                await expect(page).toHaveURL('/cookies')
            })
    
            test('The cookies banner link takes you to the cookies page', async ({ page }) => {
                await page.getByRole('link', { name: 'View cookies' }).click()
                
                await expect(page).toHaveURL('/cookies')
            })
    
            test('The hide cookies banner link takes you to the cookies page', async ({ page }) => {
                await page.getByRole('button', { name: 'Reject analytics cookies' }).click()
                await page.getByRole('link', { name: 'change your cookie setting' }).click()
                
                await expect(page).toHaveURL(/\/cookies?/)
            })
        })
    
        test.describe('Cookie value', () => {
    
            test('Clicking Accept on the cookie banner sets the cookie to the accept value', async ({ page }) => {
                await page.getByRole('button', { name: 'Accept analytics cookies' }).click()
    
                const cookies = await page.context().cookies()
                await expect(cookies.find(c => c.name == CookieName)?.value).toBe(AcceptValue)
            })
    
            test('Clicking Reject on the cookie banner sets the reject analytics cookie', async ({ page }) => {
                await page.getByRole('button', { name: 'Reject analytics cookies' }).click()
    
                const cookies = await page.context().cookies()
                await expect(cookies.find(c => c.name == CookieName)?.value).toBe(RejectValue)
            })
    
        })
    
        test.describe('Google tag manager', () => {
    
            test.skip('Accepting cookies adds the googletagmanager script', async ({ page }) => {
                await page.getByRole('button', { name: 'Accept analytics cookies' }).click()
                await page.getByRole('button', { name: 'Hide cookie message' }).click()
    
                await expect(await page.locator('script[src*=\googletagmanager\]')).toBeAttached()
            })
    
            test('Rejecting cookies does not add the googletagmanager script', async ({ page }) => {
                await page.getByRole('button', { name: 'Reject analytics cookies' }).click()
                await page.getByRole('button', { name: 'Hide cookie message' }).click()
    
                await expect(await page.locator('script[src*=\googletagmanager\]')).not.toBeAttached()
            })
    
        })

    })

    test.describe('Consent page', () => {

        test.beforeEach(async ({ page }) => {
            await page.goto('cookies')
        })

        test('Cookie banner is not shown', async ({ page }) => {
            await expect(await page.locator('#divCookieBannerId')).not.toBeVisible()
        })

        test('When cookies have not been accepted/rejected, no radio button is selected', async ({ page }) => {
            expect(await page.getByLabel('Yes').isChecked()).toBeFalsy()
            expect(await page.getByLabel('No').isChecked()).toBeFalsy()
        })

        test('When cookies have been accepted, the \'Yes\' radio button is selected', async ({ page }) => {
            await page.goto('/content')
            await page.getByRole('button', { name: 'Accept analytics cookies' }).click()
            await page.goto('/cookies')

            expect(await page.getByLabel('Yes').isChecked()).toBeTruthy()
            expect(await page.getByLabel('No').isChecked()).toBeFalsy()
        })

        test('When cookies have been rejected, the \'No\' radio button is selected', async ({ page }) => {
            await page.goto('/content')
            await page.getByRole('button', { name: 'Reject analytics cookies' }).click()
            await page.goto('/cookies')

            expect(await page.getByLabel('Yes').isChecked()).toBeFalsy()
            expect(await page.getByLabel('No').isChecked()).toBeTruthy()
        })

        test.describe('Cookie value', () => {
    
            test('Selecting \'Yes\' then clicking \'Save cookie settings\' sets the cookie to the accept value', async ({ page }) => {
                await page.getByLabel('Yes').check()
                await page.getByRole('button', { name: 'Save cookie settings' }).click()
    
                const cookies = await page.context().cookies()
                await expect(cookies.find(c => c.name == CookieName)?.value).toBe(AcceptValue)
            })

            test('Selecting \'No\' then clicking \'Save cookie settings\' sets the cookie to the reject value', async ({ page }) => {
                await page.getByLabel('No').check()
                await page.getByRole('button', { name: 'Save cookie settings' }).click()
    
                const cookies = await page.context().cookies()
                await expect(cookies.find(c => c.name == CookieName)?.value).toBe(RejectValue)
            })
    
        })
    })

})