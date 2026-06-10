# Instructions

- Following Playwright test failed.
- Explain why, be concise, respect Playwright best practices.
- Provide a snippet of code with the fix, if possible.

# Test info

- Name: asset-portal.spec.ts >> Asset Portal End-to-End Suite >> Test Equipment Depreciation Calculation
- Location: tests/asset-portal.spec.ts:49:3

# Error details

```
Error: locator.click: Element is outside of the viewport
Call log:
  - waiting for locator('button[type="submit"]')
    - locator resolved to <button tabindex="0" type="submit" data-slot="button" class="group/button inline-flex shrink-0 items-center justify-center rounded-lg border border-transparent bg-clip-padding text-sm font-medium whitespace-nowrap transition-all outline-none select-none focus-visible:border-ring focus-visible:ring-3 focus-visible:ring-ring/50 active:not-aria-[haspopup]:translate-y-px disabled:pointer-events-none disabled:opacity-50 aria-invalid:border-destructive aria-invalid:ring-3 aria-invalid:ring-destructive/20 da…>Create Asset</button>
  - attempting click action
    - scrolling into view if needed
    - done scrolling

```

# Page snapshot

```yaml
- generic [ref=e1]:
  - generic [ref=e3]:
    - complementary [ref=e4]:
      - generic [ref=e6]:
        - img [ref=e7]
        - generic [ref=e11]:
          - generic [ref=e12]: TradeOffStack
          - generic [ref=e13]: by Dordor Minetdi
      - navigation [ref=e14]:
        - link [ref=e15] [cursor=pointer]:
          - /url: /dashboard
          - img [ref=e16]
          - generic [ref=e18]: Central Hub
        - link [ref=e19] [cursor=pointer]:
          - /url: /asset-portal
          - img [ref=e20]
          - text: Dashboard
        - link [ref=e25] [cursor=pointer]:
          - /url: /inventory
          - img [ref=e26]
          - text: Inventory
        - link [ref=e29] [cursor=pointer]:
          - /url: /software
          - img [ref=e30]
          - text: Software Licenses
        - link [ref=e34] [cursor=pointer]:
          - /url: /reservations
          - img [ref=e35]
          - text: Reservations
        - link [ref=e39] [cursor=pointer]:
          - /url: /maintenance
          - img [ref=e40]
          - text: Maintenance
        - link [ref=e42] [cursor=pointer]:
          - /url: /departments
          - img [ref=e43]
          - text: Departments
        - link [ref=e47] [cursor=pointer]:
          - /url: /users
          - img [ref=e48]
          - text: Users
        - link [ref=e53] [cursor=pointer]:
          - /url: /audit-logs
          - img [ref=e54]
          - text: Audit Logs
      - button [ref=e57] [cursor=pointer]:
        - img [ref=e58]
        - text: Sign Out
    - generic [ref=e61]:
      - banner [ref=e62]:
        - generic [ref=e63]:
          - generic [ref=e64]:
            - img [ref=e66]
            - generic [ref=e69]: Asset Portal
          - button [ref=e70] [cursor=pointer]:
            - img [ref=e71]
        - generic [ref=e73]:
          - generic [ref=e74]:
            - generic [ref=e75]: System Admin
            - generic [ref=e76]: Admin
          - generic [ref=e79] [cursor=pointer]: S
      - main [ref=e80]:
        - generic [ref=e82]:
          - generic [ref=e83]:
            - generic [ref=e84]:
              - heading [level=1] [ref=e85]: Inventory
              - paragraph [ref=e86]: Manage all IT assets and equipment.
            - button [ref=e87] [cursor=pointer]:
              - img
              - text: Add Equipment
          - generic [ref=e88]:
            - generic [ref=e89]:
              - img [ref=e90]
              - textbox [ref=e93]:
                - /placeholder: Search by name or serial...
            - generic [ref=e94]:
              - combobox [ref=e95] [cursor=pointer]
              - combobox [ref=e96] [cursor=pointer]
              - generic [ref=e97]:
                - button [ref=e98] [cursor=pointer]:
                  - img
                - button [ref=e99] [cursor=pointer]:
                  - img
          - generic [ref=e100]:
            - table [ref=e102]:
              - rowgroup [ref=e103]:
                - row [ref=e104]:
                  - columnheader [ref=e105]:
                    - button [ref=e106] [cursor=pointer]:
                      - text: Asset Name
                      - img [ref=e107]
                  - columnheader [ref=e110]:
                    - button [ref=e111] [cursor=pointer]:
                      - text: Serial Number
                      - img [ref=e112]
                  - columnheader [ref=e115]:
                    - button [ref=e116] [cursor=pointer]:
                      - text: Category
                      - img [ref=e117]
                  - columnheader [ref=e120]:
                    - button [ref=e121] [cursor=pointer]:
                      - text: Status
                      - img [ref=e122]
                  - columnheader [ref=e125]:
                    - button [ref=e126] [cursor=pointer]:
                      - text: Price
                      - img [ref=e127]
              - rowgroup [ref=e130]:
                - row [ref=e131] [cursor=pointer]:
                  - cell [ref=e132]:
                    - generic [ref=e133]:
                      - img [ref=e135]
                      - text: E2E Test Laptop Pro E2E-SRL-361606
                  - cell [ref=e136]: E2E-SRL-361606
                  - cell [ref=e137]: Laptop
                  - cell [ref=e138]:
                    - generic [ref=e139]: Reserved
                  - cell [ref=e140]: $1,500.00
                - row [ref=e141] [cursor=pointer]:
                  - cell [ref=e142]:
                    - generic [ref=e143]:
                      - img [ref=e145]
                      - text: E2E Test Laptop Pro
                  - cell [ref=e146]: E2E-SRL-962543
                  - cell [ref=e147]: Laptop
                  - cell [ref=e148]:
                    - generic [ref=e149]: Available
                  - cell [ref=e150]: $1,500.00
                - row [ref=e151] [cursor=pointer]:
                  - cell [ref=e152]:
                    - generic [ref=e153]:
                      - img [ref=e155]
                      - text: Cisco Catalyst 9200L 48-port
                  - cell [ref=e156]: FOC2437X09B
                  - cell [ref=e157]: NetworkDevice
                  - cell [ref=e158]:
                    - generic [ref=e159]: Available
                  - cell [ref=e160]: $1,850.00
                - row [ref=e161] [cursor=pointer]:
                  - cell [ref=e162]:
                    - generic [ref=e163]:
                      - img [ref=e165]
                      - text: Macbook Pro 16 M3
                  - cell [ref=e166]: "12035481161561"
                  - cell [ref=e167]: Laptop
                  - cell [ref=e168]:
                    - generic [ref=e169]: Available
                  - cell [ref=e170]: $1,999.99
                - row [ref=e171] [cursor=pointer]:
                  - cell [ref=e172]:
                    - generic [ref=e173]:
                      - img [ref=e175]
                      - text: E2E Test Laptop Pro E2E-SRL-636207
                  - cell [ref=e176]: E2E-SRL-636207
                  - cell [ref=e177]: Laptop
                  - cell [ref=e178]:
                    - generic [ref=e179]: Available
                  - cell [ref=e180]: $1,500.00
                - row [ref=e181] [cursor=pointer]:
                  - cell [ref=e182]:
                    - generic [ref=e183]:
                      - img [ref=e185]
                      - text: E2E Test Laptop Pro E2E-SRL-941101
                  - cell [ref=e186]: E2E-SRL-941101
                  - cell [ref=e187]: Laptop
                  - cell [ref=e188]:
                    - generic [ref=e189]: Available
                  - cell [ref=e190]: $1,500.00
                - row [ref=e191] [cursor=pointer]:
                  - cell [ref=e192]:
                    - generic [ref=e193]:
                      - img [ref=e195]
                      - text: E2E Test Laptop Pro
                  - cell [ref=e196]: E2E-SRL-761761
                  - cell [ref=e197]: Laptop
                  - cell [ref=e198]:
                    - generic [ref=e199]: Reserved
                  - cell [ref=e200]: $1,500.00
                - row [ref=e201] [cursor=pointer]:
                  - cell [ref=e202]:
                    - generic [ref=e203]:
                      - img [ref=e205]
                      - text: E2E Test Laptop Pro E2E-SRL-82847
                  - cell [ref=e206]: E2E-SRL-82847
                  - cell [ref=e207]: Laptop
                  - cell [ref=e208]:
                    - generic [ref=e209]: Reserved
                  - cell [ref=e210]: $1,500.00
            - generic [ref=e211]:
              - generic [ref=e212]:
                - generic [ref=e213]: 1–8 of 8
                - generic [ref=e214]: "|"
                - generic [ref=e215]:
                  - generic [ref=e216]: Show
                  - combobox [ref=e217] [cursor=pointer]
                  - generic [ref=e218]: rows
              - generic [ref=e219]:
                - button [disabled]:
                  - img
                - button [ref=e220] [cursor=pointer]: "1"
                - button [disabled]:
                  - img
  - dialog "Add New IT Asset" [ref=e224]:
    - generic [ref=e225]:
      - heading "Add New IT Asset" [level=2] [ref=e226]
      - paragraph [ref=e227]: Provide the equipment information. All fields are required.
    - generic [ref=e228]:
      - generic [ref=e229]:
        - generic [ref=e230]:
          - text: Asset Name
          - textbox "e.g. MacBook Pro 16 M3" [ref=e231]: Test Depreciation Laptop
        - generic [ref=e232]:
          - text: Serial Number
          - textbox "Manufacturer Serial No." [ref=e233]: TEST-SN-507
        - generic [ref=e234]:
          - text: Category
          - combobox [ref=e235] [cursor=pointer]:
            - option "Laptop" [selected]
            - option "Monitor"
            - option "Peripheral"
            - option "Network Device"
        - generic [ref=e236]:
          - text: Status
          - combobox [ref=e237] [cursor=pointer]:
            - option "Available" [selected]
            - option "Reserved"
            - option "Repair"
            - option "Retired"
        - generic [ref=e238]:
          - text: Purchase Price ($)
          - spinbutton [ref=e239]: "200"
        - generic [ref=e240]:
          - text: Purchase Date
          - textbox [ref=e241]: 2026-06-10
        - generic [ref=e242]:
          - text: Depreciation Method
          - combobox [ref=e243] [cursor=pointer]:
            - option "None"
            - option "Straight Line" [selected]
            - option "Declining Balance"
        - generic [ref=e244]:
          - text: Salvage Value ($)
          - spinbutton [ref=e245]: "200"
        - generic [ref=e246]:
          - text: Useful Life (Years)
          - spinbutton [active] [ref=e247]: "3"
        - generic [ref=e248]:
          - text: Warranty Expiration
          - textbox [ref=e249]
        - generic [ref=e250]:
          - text: Image Source
          - generic [ref=e251]:
            - generic [ref=e252] [cursor=pointer]:
              - radio "Upload File" [checked] [ref=e253]
              - text: Upload File
            - generic [ref=e254] [cursor=pointer]:
              - radio "Image URL" [ref=e255]
              - text: Image URL
          - generic [ref=e258] [cursor=pointer]:
            - img [ref=e260]
            - paragraph [ref=e263]: Cliquez pour ajouter ou glissez-déposez
            - paragraph [ref=e264]: SVG, PNG, JPG ou GIF (MAX. 5MB)
        - generic [ref=e265]:
          - text: Description
          - textbox "Describe specifications or department assignment" [ref=e266]
      - generic [ref=e267]:
        - button "Cancel" [ref=e268] [cursor=pointer]
        - button "Create Asset" [ref=e269] [cursor=pointer]
    - button "Close" [ref=e270]:
      - img
      - generic [ref=e271]: Close
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
  13 |     await expect(page.locator('text=Central Hub').or(page.locator('text=Accueil central'))).toBeVisible({ timeout: 10000 });
  14 |   });
  15 | 
  16 |   test('Seed and Validate Software Licenses', async ({ page }) => {
  17 |     // Navigate to Software page
  18 |     await page.goto('/software');
  19 |     
  20 |     // Expect the header
  21 |     await expect(page.locator('h1')).toContainText('Software Licenses', { ignoreCase: true, timeout: 5000 }).catch(() => 
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
  36 |       await page.fill('input[placeholder*="Office 365"]', lic.name);
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
  54 |     await page.locator('button:has-text("Add"), button:has-text("Ajouter")').first().click();
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
> 73 |     await page.locator('button[type="submit"]').click({ force: true });
     |                                                 ^ Error: locator.click: Element is outside of the viewport
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