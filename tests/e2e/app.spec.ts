import { test, expect } from '@playwright/test';

test('página inicial carrega', async ({ page }) => {
  await page.goto('/');
  const body = await page.textContent('body');
  expect(body).toContain('Minhas Finanças');
});
