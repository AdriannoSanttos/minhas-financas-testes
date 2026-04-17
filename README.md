# Testes Minhas Finanças

## Executar testes unitários
cd tests/unit/MinhasFinancas.UnitTests
dotnet test

## Executar testes de integração
cd tests/integration/MinhasFinancas.IntegrationTests
dotnet test

## Executar testes E2E
cd tests/e2e
npx playwright test

## Regras testadas
- Menor de idade não pode ter receitas
- Categoria compatível com tipo da transação
- Exclusão em cascata

## Resultados
Todos os testes passam.
