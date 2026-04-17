import { test, expect } from '@playwright/test';

test('página inicial carrega', async ({ page }) => {
  await page.goto('/');
  await expect(page).toHaveTitle(/Minhas Finanças/);
});

test('navegação para página de pessoas funciona', async ({ page }) => {
  await page.goto('/');
  await page.click('text=Pessoas');
  await expect(page).toHaveURL(/pessoas/);
  await expect(page.locator('h1')).toContainText(/Pessoas/i);
});
