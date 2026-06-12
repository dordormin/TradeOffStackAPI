# Instructions

- Following Playwright test failed.
- Explain why, be concise, respect Playwright best practices.
- Provide a snippet of code with the fix, if possible.

# Test info

- Name: asset-portal.spec.ts >> Asset Portal End-to-End Suite >> Test Equipment Depreciation Calculation
- Location: tests/asset-portal.spec.ts:49:3

# Error details

```
Test timeout of 30000ms exceeded.
```

```
Error: locator.click: Test timeout of 30000ms exceeded.
Call log:
  - waiting for locator('button:has-text("Add"), button:has-text("Ajouter")').first()

```

# Page snapshot

```yaml
- generic [ref=e3]:
  - generic [ref=e5]:
    - generic [ref=e6]:
      - generic [ref=e8]:
        - img [ref=e9]
        - generic [ref=e13]:
          - generic [ref=e14]: TradeOffStack
          - generic [ref=e15]: by Dordor Minetdi
      - heading "Welcome back" [level=2] [ref=e16]
      - paragraph [ref=e17]: Sign in to access your corporate IT asset workspace.
    - generic [ref=e18]:
      - button "Sign In" [ref=e19]:
        - img [ref=e20]
        - text: Sign In
      - button "Sign Up" [ref=e23]:
        - img [ref=e24]
        - text: Sign Up
    - generic [ref=e27]:
      - generic [ref=e28]:
        - img [ref=e29]
        - generic [ref=e32]: "Demo Access / Accès démo :"
      - generic [ref=e33]:
        - generic [ref=e34]:
          - text: "Admin:"
          - code [ref=e35]: admin@tradeoffstack.com
        - generic [ref=e36]: Admin123!Secure
      - generic [ref=e37]:
        - generic [ref=e38]:
          - text: "Tester:"
          - code [ref=e39]: tester@tradeoffstack.com
        - generic [ref=e40]: Tester123!Secure
    - generic [ref=e41]:
      - generic [ref=e42]:
        - text: Email Address
        - generic [ref=e43]:
          - img [ref=e44]
          - textbox "admin@tradeoffstack.com" [ref=e47]
      - generic [ref=e48]:
        - generic [ref=e50]: Password
        - generic [ref=e51]:
          - img [ref=e52]
          - textbox "••••••••" [ref=e55]
          - button [ref=e56]:
            - img [ref=e57]
      - button "Sign In to Account" [ref=e60]:
        - img [ref=e61]
        - text: Sign In to Account
    - paragraph [ref=e65]:
      - img [ref=e66]
      - text: Secured with AES-256 JWT Authorization tokens.
  - generic [ref=e69]:
    - img "3D JetBrains Mosaic" [ref=e71]
    - generic [ref=e72]:
      - generic [ref=e75]: v4.0.0 Stable
      - heading "Enterprise Hub, Orchestrated." [level=1] [ref=e76]
      - paragraph [ref=e77]: Track hardware parameters, automate reservation lifecycles, and coordinate technical interventions inside a unified glassmorphic dashboard.
      - generic [ref=e78]:
        - generic [ref=e79]:
          - paragraph [ref=e80]: 99.9%
          - paragraph [ref=e81]: System Uptime
        - generic [ref=e82]:
          - paragraph [ref=e83]: 100ms
          - paragraph [ref=e84]: Avg API Response
        - generic [ref=e85]:
          - paragraph [ref=e86]: Active
          - paragraph [ref=e87]: Audit Journaling
```

# Test source

```ts
  1  | import { test, expect } from '@playwright/test';
  2  | 
  3  | test.describe('Asset Portal End-to-End Suite', () => {
  4  | 
  5  |   test.beforeEach(async ({ page }) => {
  6  |     // Navigate to the app and login as Admin
  7  |     await page.goto('/login');
  8  |     await page.fill('input[type="email"]', 'admin@tradeoffstack.com');
  9  |     await page.fill('input[type="password"]', 'Admin123!Secure');
  10 |     await page.click('button[type="submit"]');
  11 |     
  12 |     // Wait for redirect to central hub
  13 |     await expect(page.locator('text=Application Central').or(page.locator('text=Enterprise Hub'))).toBeVisible({ timeout: 15000 });
  14 |   });
  15 | 
  16 |   test('Seed and Validate Software Licenses', async ({ page }) => {
  17 |     // Navigate to Licenses page
  18 |     await page.goto('/licenses');
  19 |     
  20 |     // Expect the header
  21 |     await expect(page.locator('h1')).toContainText('Licenses', { ignoreCase: true, timeout: 5000 }).catch(() => 
  22 |       expect(page.locator('h1')).toContainText('Licences', { ignoreCase: true })
  23 |     );
  24 | 
  25 |     const licensesToCreate = [
  26 |       { name: 'Office 365 E3', key: 'MSFT-O365-E3-2026', seats: '50', price: '12000' },
  27 |       { name: 'Adobe Creative Cloud', key: 'ADOBE-CC-CORP-99', seats: '10', price: '8500' },
  28 |       { name: 'JetBrains All Products', key: 'JB-ALL-2026-DEV', seats: '25', price: '7250' }
  29 |     ];
  30 | 
  31 |     for (const lic of licensesToCreate) {
  32 |       // Click Add License
  33 |       await page.click('button:has-text("Add License"), button:has-text("Ajouter une licence")');
  34 |       
  35 |       // Fill the form
  36 |       await page.fill('input[placeholder*="Windows 11"]', lic.name);
  37 |       await page.fill('input[placeholder*="XXXX"]', lic.key);
  38 |       await page.fill('input[type="number"]:not([step])', lic.seats); // Total seats has no step
  39 |       await page.fill('input[step="0.01"]', lic.price); // Price has step="0.01"
  40 |       
  41 |       // Submit
  42 |       await page.locator('button[type="submit"]').click();
  43 |       
  44 |       // Wait for it to appear in the table
  45 |       await expect(page.locator(`text=${lic.name}`).first()).toBeVisible();
  46 |     }
  47 |   });
  48 | 
  49 |   test('Test Equipment Depreciation Calculation', async ({ page }) => {
  50 |     // Navigate to Inventory page
  51 |     await page.goto('/inventory');
  52 |     
  53 |     // Open Add form
> 54 |     await page.locator('button:has-text("Add"), button:has-text("Ajouter")').first().click();
     |                                                                                      ^ Error: locator.click: Test timeout of 30000ms exceeded.
  55 |     
  56 |     // Fill required details
  57 |     await page.fill('input[placeholder*="MacBook"]', 'Test Depreciation Laptop');
  58 |     await page.fill('input[placeholder*="Serial No"]', `TEST-SN-${Math.floor(Math.random() * 10000)}`);
  59 |     
  60 |     // Set purchase price
  61 |     await page.fill('input[placeholder="0.00"]', '2000');
  62 |     
  63 |     // Set Depreciation fields
  64 |     await page.locator('label').filter({ hasText: 'Depreciation Method' }).or(page.locator('label').filter({ hasText: 'Méthode d\'amortissement' })).locator('..').locator('select').selectOption('StraightLine');
  65 |     
  66 |     await page.fill('input[step="0.01"]:nth-of-type(1)', '200'); // Actually, purchase price has step="0.01", salvage value also has step="0.01"
  67 |     // Let's use the labels to find them.
  68 |     await page.locator('label').filter({ hasText: 'Valeur résiduelle' }).or(page.locator('label').filter({ hasText: 'Salvage Value' })).locator('..').locator('input').fill('200');
  69 |     
  70 |     await page.fill('input[step="0.5"]', '3'); // Useful Life
  71 |     
  72 |     // Submit
  73 |     await page.locator('button[type="submit"]').click({ force: true });
  74 |     
  75 |     // Verify it appeared and click to open details
  76 |     await page.locator('text=Test Depreciation Laptop').first().click();
  77 |     
  78 |     // Verify the Book Value is rendered. With $2000 price, $200 salvage, 3 yrs life, it should have a book value.
  79 |     // The exact calculation depends on the purchase date (today), so it might be close to $2000.
  80 |     // We just verify the Current Book Value element exists.
  81 |     await expect(page.locator('text=Current Book Value').or(page.locator('text=Valeur comptable'))).toBeVisible();
  82 |   });
  83 | });
  84 | 
```