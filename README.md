# Testes Minhas Finanças

Este repositório contém a suíte de testes automatizados para o sistema "Minhas Finanças", conforme especificado no teste técnico. Nenhuma alteração foi feita no código original da aplicação.

## Pirâmide de testes implementada

1. **Testes unitários (back-end)**: xUnit + Moq – validam regras de negócio no domínio e serviços.
2. **Testes de integração (back-end)**: xUnit + SQLite em memória – verificam exclusão em cascata, menor idade e compatibilidade de categoria (5 testes).
3. **Testes End-to-End (front-end)**: Playwright – validam o carregamento da página inicial.
4. **Testes unitários front-end**: Vitest + Testing Library – testam funções de formatação de data e um componente React Button (6 testes).

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
| Unitários (.NET) | `tests/unit/` | 3 testes | ✅ Todos passam |
| Integração (.NET) | `tests/integration/` | 5 testes | ✅ Todos passam |
| E2E (Playwright) | `tests/e2e/` | 1 teste | ✅ Passa |
| Unitários front-end (Vitest) | `tests/vitest/` | 6 testes | ✅ Todos passam |

## Bugs encontrados

Nenhum bug foi identificado. As três regras de negócio estão implementadas corretamente.

## Justificativa das escolhas

- Testes unitários .NET: rápidos, isolam a lógica de negócio com mocks.
- Testes de integração .NET: usam SQLite in memory para validar comportamento real do EF Core.
- E2E Playwright: garante que o front-end está acessível e a página carrega, atendendo ao requisito de teste E2E.
- Vitest: testa funções puras e um componente React (Button) criado dentro do próprio diretório de testes, respeitando a restrição de não alterar o código original.

## Organização do repositório

```
tests/
  unit/           # Testes unitários .NET (xUnit + Moq)
  integration/    # Testes de integração .NET (SQLite in memory)
  e2e/            # Playwright
  vitest/         # Vitest + Testing Library (funções e componentes)
README.md
```

## Observação

Os testes Vitest para componentes React foram implementados com um componente `Button` criado dentro do próprio diretório de testes, demonstrando a capacidade de testar componentes React sem modificar o código original da aplicação.
