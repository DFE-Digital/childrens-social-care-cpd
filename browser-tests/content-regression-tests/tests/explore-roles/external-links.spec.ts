import { test, expect } from '@playwright/test'

test.describe('External links @external', () => {
    test.describe('Newly qualified social worker', () => {
        const links = [
            ['Social Work England', 'https://www.socialworkengland.org.uk/education-training/search-approved-courses/'],
            ['social work bursary', 'https://www.gov.uk/social-work-bursaries'],
            ['Social Worker Level 6 Degree Apprenticeship', 'https://findapprenticeshiptraining.apprenticeships.education.gov.uk/courses/381'],
            ['Frontline', 'https://thefrontline.org.uk/become-a-social-worker/frontline-programme/'],
            ['Step Up to Social Work', 'https://www.gov.uk/guidance/step-up-to-social-work'],
            ['Find a job service', 'https://findajob.dwp.gov.uk/'],
        ]
        
        for (const link of links) {
            test(`'${link[0]}' goes to ${link[1]}`, async ({ page }) => {
                await page.goto('/explore-roles/newly-qualified-social-worker')
                await page.getByRole('button', { name: 'Show all sections' }).click()
                var promise = page.waitForResponse(`**${link[1]}`)
                await page.getByRole('link', { name: link[0], exact: true }).click()
                var response = await promise
                
                expect(response.ok()).toBeTruthy()
            })
        }
    })

    test.describe('Social worker', () => {
        const links = [
            ['Find a job service', 'https://findajob.dwp.gov.uk/'],
        ]
        
        for (const link of links) {
            test(`'${link[0]}' goes to ${link[1]}`, async ({ page }) => {
                await page.goto('/explore-roles/social-worker')
                await page.getByRole('button', { name: 'Show all sections' }).click()
                var promise = page.waitForResponse(`**${link[1]}`)
                await page.getByRole('link', { name: link[0], exact: true }).click()
                var response = await promise
                
                expect(response.ok()).toBeTruthy()
            })
        }
    })

    test.describe('Senior practitioner', () => {
        const links = [
            ['Find a job service', 'https://findajob.dwp.gov.uk/'],
        ]
        
        for (const link of links) {
            test(`'${link[0]}' goes to ${link[1]}`, async ({ page }) => {
                await page.goto('/explore-roles/senior-practitioner')
                await page.getByRole('button', { name: 'Show all sections' }).click()
                var promise = page.waitForResponse(`**${link[1]}`)
                await page.getByRole('link', { name: link[0], exact: true }).click()
                var response = await promise
                
                expect(response.ok()).toBeTruthy()
            })
        }
    })

    test.describe('Practice supervisor', () => {
        const links = [
            ['enhanced background checks', 'https://www.gov.uk/criminal-record-checks-apply-role'],
            ['Find a job service', 'https://findajob.dwp.gov.uk/'],
        ]
        
        for (const link of links) {
            test(`'${link[0]}' goes to ${link[1]}`, async ({ page }) => {
                await page.goto('/explore-roles/practice-supervisor')
                await page.getByRole('button', { name: 'Show all sections' }).click()
                var promise = page.waitForResponse(`**${link[1]}`)
                await page.getByRole('link', { name: link[0], exact: true }).click()
                var response = await promise
                
                expect(response.ok()).toBeTruthy()
            })
        }
    })

    test.describe('Practice development manager', () => {
        const links = [
            ['Find a job service', 'https://findajob.dwp.gov.uk/'],
        ]
        
        for (const link of links) {
            test(`'${link[0]}' goes to ${link[1]}`, async ({ page }) => {
                await page.goto('/explore-roles/practice-development-manager')
                await page.getByRole('button', { name: 'Show all sections' }).click()
                var promise = page.waitForResponse(`**${link[1]}`)
                await page.getByRole('link', { name: link[0], exact: true }).click()
                var response = await promise
                
                expect(response.ok()).toBeTruthy()
            })
        }
    })

    test.describe('Quality improvement manager', () => {
        const links = [
            ['Find a job service', 'https://findajob.dwp.gov.uk/'],
        ]
        
        for (const link of links) {
            test(`'${link[0]}' goes to ${link[1]}`, async ({ page }) => {
                await page.goto('/explore-roles/quality-improvement-manager')
                await page.getByRole('button', { name: 'Show all sections' }).click()
                var promise = page.waitForResponse(`**${link[1]}`)
                await page.getByRole('link', { name: link[0], exact: true }).click()
                var response = await promise
                
                expect(response.ok()).toBeTruthy()
            })
        }
    })

    test.describe('Team manager', () => {
        const links = [
            ['Find a job service', 'https://findajob.dwp.gov.uk/'],
        ]
        
        for (const link of links) {
            test(`'${link[0]}' goes to ${link[1]}`, async ({ page }) => {
                await page.goto('/explore-roles/team-manager')
                await page.getByRole('button', { name: 'Show all sections' }).click()
                var promise = page.waitForResponse(`**${link[1]}`)
                await page.getByRole('link', { name: link[0], exact: true }).click()
                var response = await promise
                
                expect(response.ok()).toBeTruthy()
            })
        }
    })

    test.describe('Independent reviewing officer', () => {
        const links = [
            ['Find a job service', 'https://findajob.dwp.gov.uk/'],
        ]
        
        for (const link of links) {
            test(`'${link[0]}' goes to ${link[1]}`, async ({ page }) => {
                await page.goto('/explore-roles/independent-reviewing-officer')
                await page.getByRole('button', { name: 'Show all sections' }).click()
                var promise = page.waitForResponse(`**${link[1]}`)
                await page.getByRole('link', { name: link[0], exact: true }).click()
                var response = await promise
                
                expect(response.ok()).toBeTruthy()
            })
        }
    })

    test.describe('Principal social worker', () => {
        const links = [
            ['enhanced background checks', 'https://www.gov.uk/criminal-record-checks-apply-role'],
            ['Find a job service', 'https://findajob.dwp.gov.uk/'],
        ]
        
        for (const link of links) {
            test(`'${link[0]}' goes to ${link[1]}`, async ({ page }) => {
                await page.goto('/explore-roles/principal-social-worker')
                await page.getByRole('button', { name: 'Show all sections' }).click()
                var promise = page.waitForResponse(`**${link[1]}`)
                await page.getByRole('link', { name: link[0], exact: true }).click()
                var response = await promise
                
                expect(response.ok()).toBeTruthy()
            })
        }
    })

    test.describe('Service manager', () => {
        const links = [
            ['enhanced background checks', 'https://www.gov.uk/criminal-record-checks-apply-role'],
            ['Find a job service', 'https://findajob.dwp.gov.uk/'],
        ]
        
        for (const link of links) {
            test(`'${link[0]}' goes to ${link[1]}`, async ({ page }) => {
                await page.goto('/explore-roles/service-manager')
                await page.getByRole('button', { name: 'Show all sections' }).click()
                var promise = page.waitForResponse(`**${link[1]}`)
                await page.getByRole('link', { name: link[0], exact: true }).click()
                var response = await promise
                
                expect(response.ok()).toBeTruthy()
            })
        }
    })

    test.describe('Head of service', () => {
        const links = [
            ['enhanced background checks', 'https://www.gov.uk/criminal-record-checks-apply-role'],
            ['Find a job service', 'https://findajob.dwp.gov.uk/'],
        ]
        
        for (const link of links) {
            test(`'${link[0]}' goes to ${link[1]}`, async ({ page }) => {
                await page.goto('/explore-roles/head-of-service')
                await page.getByRole('button', { name: 'Show all sections' }).click()
                var promise = page.waitForResponse(`**${link[1]}`)
                await page.getByRole('link', { name: link[0], exact: true }).click()
                var response = await promise
                
                expect(response.ok()).toBeTruthy()
            })
        }
    })

    test.describe('Assistant director', () => {
        const links = [
            ['enhanced background checks', 'https://www.gov.uk/criminal-record-checks-apply-role'],
            ['Find a job service', 'https://findajob.dwp.gov.uk/'],
        ]
        
        for (const link of links) {
            test(`'${link[0]}' goes to ${link[1]}`, async ({ page }) => {
                await page.goto('/explore-roles/assistant-director')
                await page.getByRole('button', { name: 'Show all sections' }).click()
                var promise = page.waitForResponse(`**${link[1]}`)
                await page.getByRole('link', { name: link[0], exact: true }).click()
                var response = await promise
                
                expect(response.ok()).toBeTruthy()
            })
        }
    })

    test.describe('Director of children\'s services', () => {
        const links = [
            ['enhanced background checks', 'https://www.gov.uk/criminal-record-checks-apply-role'],
            ['Find a job service', 'https://findajob.dwp.gov.uk/'],
        ]
        
        for (const link of links) {
            test(`'${link[0]}' goes to ${link[1]}`, async ({ page }) => {
                await page.goto('/explore-roles/director-children-services')
                await page.getByRole('button', { name: 'Show all sections' }).click()
                var promise = page.waitForResponse(`**${link[1]}`)
                await page.getByRole('link', { name: link[0], exact: true }).click()
                var response = await promise
                
                expect(response.ok()).toBeTruthy()
            })
        }
    })
})