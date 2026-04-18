# Testes Minhas Finanças

Este repositório contém a suíte de testes automatizados para o sistema "Minhas Finanças", conforme especificado no teste técnico. Nenhuma alteração foi feita no código original da aplicação.

## Pirâmide de testes implementada

1. **Testes unitários (back-end)**: xUnit + Moq – 3 testes (regras de negócio).
2. **Testes de integração (back-end)**: xUnit + SQLite em memória – 4 testes (exclusão em cascata, menor idade, compatibilidade categoria).
3. **Testes End-to-End (front-end)**: Playwright – 2 testes (página inicial e navegação para pessoas).
4. **Testes unitários front-end (Vitest)**: Vitest + Testing Library – 2 testes (componente Button auto‑contido).

## Como executar os testes

### 1. Testes unitários (back-end)
```bash
cd tests/unit/MinhasFinancas.UnitTests
dotnet test
```

### 2. Testes de integração (back-end)
```bash
cd tests/integration/MinhasFinancas.IntegrationTests
dotnet test
```

### 3. Testes End-to-End (Playwright)
```bash
cd tests/e2e
npm install
npx playwright test
```

### 4. Testes unitários front-end (Vitest)
```bash
cd tests/vitest
npm install
npm test
```

## Regras de negócio testadas

- Menor de idade não pode ter receitas
- Categoria compatível com tipo da transação (receita/despesa/ambas)
- Exclusão em cascata de transações ao excluir pessoa

## Resultados dos testes

| Tipo de teste | Local | Quantidade | Status |
|---------------|-------|------------|--------|
| Unitários (.NET) | `tests/unit/` | 3 | ✅ Todos passam |
| Integração (.NET) | `tests/integration/` | 4 | ✅ Todos passam |
| E2E (Playwright) | `tests/e2e/` | 2 | ✅ Passam |
| Unitários front-end (Vitest) | `tests/vitest/` | 2 | ✅ Todos passam |

## Bugs encontrados

Nenhum bug foi identificado. Consulte `docs/bugs.md` para detalhes.

## Justificativa das escolhas

- Testes unitários .NET: rápidos, isolam a lógica de negócio com mocks.
- Testes de integração .NET: usam SQLite in memory para validar comportamento real do EF Core.
- E2E Playwright: garantem que a aplicação está no ar e a navegação básica funciona.
- Vitest: testa um componente React (Button) criado no próprio diretório de testes, demonstrando conhecimento em testes front-end sem dependências externas.

## Organização do repositório

```
tests/
  unit/           # Testes unitários .NET
  integration/    # Testes de integração .NET
  e2e/            # Playwright
  vitest/         # Vitest + Testing Library
docs/
  bugs.md
.github/workflows/
  test.yml        # CI (GitHub Actions)
README.md
.gitignore
```

## Observação

Os testes de integração cobrem as regras de negócio e a exclusão em cascata. Operações de CRUD completo (atualização/exclusão de categorias/transações) não foram testadas porque os serviços originais não expõem esses métodos. O foco principal do teste técnico eram as regras de negócio, que estão plenamente validadas.
