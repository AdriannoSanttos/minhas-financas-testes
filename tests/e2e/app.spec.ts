import { test, expect } from '@playwright/test';

test('página inicial carrega', async ({ page }) => {
  await page.goto('http://localhost:5173');
  await expect(page.locator('body')).toContainText('Minhas Finanças');
});

test('navegação para página de pessoas', async ({ page }) => {
  await page.goto('http://localhost:5173');
  // Usa getByRole com expressão regular para maior robustez
  await page.getByRole('link', { name: /pessoas/i }).click();
  await expect(page).toHaveURL(/pessoas/);
});
