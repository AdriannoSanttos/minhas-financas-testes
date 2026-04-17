import { test, expect } from '@playwright/test';

test('página inicial carrega', async ({ page }) => {
  await page.goto('/');
  await expect(page.locator('body')).toContainText('Minhas Finanças');
});

test('navegação para página de pessoas', async ({ page }) => {
  await page.goto('/');
  await page.click('text=Pessoas');
  await expect(page).toHaveURL(/pessoas/);
});
