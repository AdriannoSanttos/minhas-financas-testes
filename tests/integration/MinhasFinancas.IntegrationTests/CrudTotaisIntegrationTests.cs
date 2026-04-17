using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MinhasFinancas.Application.DTOs;
using MinhasFinancas.Application.Services;
using MinhasFinancas.Domain.Entities;
using MinhasFinancas.Infrastructure.Data;
using MinhasFinancas.Infrastructure;
using Xunit;

namespace MinhasFinancas.IntegrationTests
{
    public class CrudTotaisIntegrationTests : IDisposable
    {
        private readonly MinhasFinancasDbContext _context;
        private readonly UnitOfWork _unitOfWork;
        private readonly PessoaService _pessoaService;
        private readonly CategoriaService _categoriaService;
        private readonly TransacaoService _transacaoService;

        public CrudTotaisIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<MinhasFinancasDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            _context = new MinhasFinancasDbContext(options);
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();
            _unitOfWork = new UnitOfWork(_context);
            _pessoaService = new PessoaService(_unitOfWork);
            _categoriaService = new CategoriaService(_unitOfWork);
            _transacaoService = new TransacaoService(_unitOfWork);
        }


        [Fact]
        public async Task Pessoa_Criar_Get_Update_Delete()
        {
            var criarDto = new CreatePessoaDto { Nome = "João", DataNascimento = DateTime.Today.AddYears(-20) };
            var criada = await _pessoaService.CreateAsync(criarDto);
            Assert.NotNull(criada);
            Assert.Equal("João", criada.Nome);

            var obtida = await _pessoaService.GetByIdAsync(criada.Id);
            Assert.NotNull(obtida);
            Assert.Equal(criada.Id, obtida.Id);

            var updateDto = new UpdatePessoaDto { Nome = "João Atualizado", DataNascimento = DateTime.Today.AddYears(-21) };
            await _pessoaService.UpdateAsync(criada.Id, updateDto);
            var atualizada = await _pessoaService.GetByIdAsync(criada.Id);
            Assert.Equal("João Atualizado", atualizada.Nome);

            await _pessoaService.DeleteAsync(criada.Id);
            var deletada = await _pessoaService.GetByIdAsync(criada.Id);
            Assert.Null(deletada);
        }


        [Fact]
        public async Task Categoria_Criar_E_Obter()
        {
            var criarDto = new CreateCategoriaDto { Descricao = "Categoria Teste", Finalidade = Categoria.EFinalidade.Ambas };
            var criada = await _categoriaService.CreateAsync(criarDto);
            Assert.NotNull(criada);
            Assert.Equal("Categoria Teste", criada.Descricao);

            var obtida = await _categoriaService.GetByIdAsync(criada.Id);
            Assert.NotNull(obtida);
            Assert.Equal(criada.Id, obtida.Id);
        }

        [Fact]
        public async Task Transacao_Criar_E_Obter()
        {
            var pessoa = await _pessoaService.CreateAsync(new CreatePessoaDto { Nome = "Pessoa Tx", DataNascimento = DateTime.Today.AddYears(-20) });
            var categoria = await _categoriaService.CreateAsync(new CreateCategoriaDto { Descricao = "Cat Tx", Finalidade = Categoria.EFinalidade.Ambas });

            var criarDto = new CreateTransacaoDto
            {
                Descricao = "Transação Teste",
                Valor = 100,
                Data = DateTime.Today,
                Tipo = Transacao.ETipo.Despesa,
                CategoriaId = categoria.Id,
                PessoaId = pessoa.Id
            };
            var criada = await _transacaoService.CreateAsync(criarDto);
            Assert.NotNull(criada);
            Assert.Equal(100, criada.Valor);

            var obtida = await _transacaoService.GetByIdAsync(criada.Id);
            Assert.NotNull(obtida);
            Assert.Equal(criada.Id, obtida.Id);
        }

        [Fact]
        public async Task TotaisPorPessoa_DeveCalcular()
        {
            var pessoa = await _pessoaService.CreateAsync(new CreatePessoaDto { Nome = "Pessoa Total", DataNascimento = DateTime.Today.AddYears(-20) });
            var categoria = await _categoriaService.CreateAsync(new CreateCategoriaDto { Descricao = "Cat Total", Finalidade = Categoria.EFinalidade.Ambas });

            var receita = new CreateTransacaoDto
            {
                Descricao = "Receita",
                Valor = 1000,
                Data = DateTime.Today,
                Tipo = Transacao.ETipo.Receita,
                CategoriaId = categoria.Id,
                PessoaId = pessoa.Id
            };
            var despesa = new CreateTransacaoDto
            {
                Descricao = "Despesa",
                Valor = 300,
                Data = DateTime.Today,
                Tipo = Transacao.ETipo.Despesa,
                CategoriaId = categoria.Id,
                PessoaId = pessoa.Id
            };
            await _transacaoService.CreateAsync(receita);
            await _transacaoService.CreateAsync(despesa);

            var totalReceitas = await _context.Transacoes
                .Where(t => t.PessoaId == pessoa.Id && t.Tipo == Transacao.ETipo.Receita)
                .SumAsync(t => t.Valor);
            var totalDespesas = await _context.Transacoes
                .Where(t => t.PessoaId == pessoa.Id && t.Tipo == Transacao.ETipo.Despesa)
                .SumAsync(t => t.Valor);
            var saldo = totalReceitas - totalDespesas;

            Assert.Equal(1000, totalReceitas);
            Assert.Equal(300, totalDespesas);
            Assert.Equal(700, saldo);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
