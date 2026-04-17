# Testes Minhas Finanças

Este repositório contém a suíte de testes automatizados para o sistema "Minhas Finanças", conforme especificado no teste técnico. Nenhuma alteração foi feita no código original da aplicação.

## Pirâmide de testes implementada

1. **Testes unitários (back-end)**: xUnit + Moq – validam regras de negócio no domínio e serviços.
2. **Testes de integração (back-end)**: xUnit + SQLite em memória – verificam a exclusão em cascata e interações com o banco.
3. **Testes End-to-End (front-end)**: Playwright – navegador real para validar fluxos completos.
4. **Testes unitários front-end (funções puras)**: Vitest – testam funções auxiliares de formatação de data.

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
- Categoria compatível com tipo da transação
- Exclusão em cascata de transações ao excluir pessoa

## Resultados

Todos os testes passam. As três regras estão implementadas corretamente.

## Bugs encontrados

Nenhum bug foi identificado.

## Justificativa das escolhas

- Priorizou-se testes unitários para validação rápida das regras.
- Testes de integração com SQLite em memória garantem o comportamento do EF Core.
- E2E com Playwright cobre o front-end sem dependência de implementação interna.
- Vitest testa funções puras do front-end, respeitando a restrição de não alterar o código original.

## Organização do repositório

```
tests/
  unit/           # Testes unitários .NET
  integration/    # Testes de integração .NET
  e2e/            # Playwright
  vitest/         # Vitest (funções puras)
README.md
```

## Observação

Os testes Vitest foram implementados apenas para funções puras (formatação de data) devido à complexidade de isolar componentes React sem modificar o código original.
