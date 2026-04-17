import { describe, it, expect } from 'vitest';
import { formatDate, formatDateForInput } from '../../../Exame Técnico/ExameDesenvolvedorDeTestes/web/src/lib/formatters';

describe('formatters', () => {
  it('formatDate retorna vazio para data nula', () => {
    expect(formatDate(null)).toBe('');
  });

  it('formatDate formata data para pt-BR', () => {
    const date = new Date(2025, 0, 1);
    expect(formatDate(date)).toBe('01/01/2025');
  });

  it('formatDateForInput retorna vazio para data nula', () => {
    expect(formatDateForInput(null)).toBe('');
  });

  it('formatDateForInput formata para yyyy-mm-dd', () => {
    const date = new Date(2025, 0, 1);
    expect(formatDateForInput(date)).toBe('2025-01-01');
  });
});
