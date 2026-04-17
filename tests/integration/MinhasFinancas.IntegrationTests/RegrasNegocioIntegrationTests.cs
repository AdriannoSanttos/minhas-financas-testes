using System;
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
    public class RegrasNegocioIntegrationTests : IDisposable
    {
        private readonly MinhasFinancasDbContext _context;
        private readonly UnitOfWork _unitOfWork;
        private readonly TransacaoService _transacaoService;

        public RegrasNegocioIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<MinhasFinancasDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            _context = new MinhasFinancasDbContext(options);
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();
            _unitOfWork = new UnitOfWork(_context);
            _transacaoService = new TransacaoService(_unitOfWork);
        }

        [Fact]
        public async Task CriarReceita_PessoaMenorIdade_DeveLancarExcecao()
        {
            var pessoa = new Pessoa { Nome = "Menor", DataNascimento = DateTime.Today.AddYears(-16) };
            await _context.Pessoas.AddAsync(pessoa);
            var categoria = new Categoria { Descricao = "Salario", Finalidade = Categoria.EFinalidade.Receita };
            await _context.Categorias.AddAsync(categoria);
            await _unitOfWork.SaveChangesAsync();

            var dto = new CreateTransacaoDto
            {
                Descricao = "Salario",
                Valor = 100,
                Data = DateTime.Today,
                Tipo = Transacao.ETipo.Receita,
                CategoriaId = categoria.Id,
                PessoaId = pessoa.Id
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() => _transacaoService.CreateAsync(dto));
        }

        [Fact]
        public async Task CriarReceita_CategoriaApenasDespesa_DeveLancarExcecao()
        {
            var pessoa = new Pessoa { Nome = "Adulto", DataNascimento = DateTime.Today.AddYears(-25) };
            await _context.Pessoas.AddAsync(pessoa);
            var categoria = new Categoria { Descricao = "Lazer", Finalidade = Categoria.EFinalidade.Despesa };
            await _context.Categorias.AddAsync(categoria);
            await _unitOfWork.SaveChangesAsync();

            var dto = new CreateTransacaoDto
            {
                Descricao = "Salario",
                Valor = 100,
                Data = DateTime.Today,
                Tipo = Transacao.ETipo.Receita,
                CategoriaId = categoria.Id,
                PessoaId = pessoa.Id
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() => _transacaoService.CreateAsync(dto));
        }

        [Fact]
        public async Task CriarDespesa_CategoriaApenasReceita_DeveLancarExcecao()
        {
            var pessoa = new Pessoa { Nome = "Adulto", DataNascimento = DateTime.Today.AddYears(-25) };
            await _context.Pessoas.AddAsync(pessoa);
            var categoria = new Categoria { Descricao = "Bonus", Finalidade = Categoria.EFinalidade.Receita };
            await _context.Categorias.AddAsync(categoria);
            await _unitOfWork.SaveChangesAsync();

            var dto = new CreateTransacaoDto
            {
                Descricao = "Despesa",
                Valor = 50,
                Data = DateTime.Today,
                Tipo = Transacao.ETipo.Despesa,
                CategoriaId = categoria.Id,
                PessoaId = pessoa.Id
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() => _transacaoService.CreateAsync(dto));
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
