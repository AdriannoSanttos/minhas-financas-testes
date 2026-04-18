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
    public class PessoaCrudIntegrationTests : IDisposable
    {
        private readonly MinhasFinancasDbContext _context;
        private readonly UnitOfWork _unitOfWork;
        private readonly PessoaService _pessoaService;

        public PessoaCrudIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<MinhasFinancasDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            _context = new MinhasFinancasDbContext(options);
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();
            _unitOfWork = new UnitOfWork(_context);
            _pessoaService = new PessoaService(_unitOfWork);
        }

        [Fact]
        public async Task AtualizarPessoa_DeveModificarDados()
        {
            var criarDto = new CreatePessoaDto { Nome = "Original", DataNascimento = DateTime.Today.AddYears(-20) };
            var criada = await _pessoaService.CreateAsync(criarDto);
            Assert.NotNull(criada);

            var updateDto = new UpdatePessoaDto { Nome = "Atualizada", DataNascimento = DateTime.Today.AddYears(-21) };
            await _pessoaService.UpdateAsync(criada.Id, updateDto);
            var atualizada = await _pessoaService.GetByIdAsync(criada.Id);

            Assert.Equal("Atualizada", atualizada.Nome);
            Assert.Equal(DateTime.Today.AddYears(-21).Date, atualizada.DataNascimento.Date);
        }

        [Fact]
        public async Task ExcluirPessoa_DeveRemoverRegistro()
        {
            var criarDto = new CreatePessoaDto { Nome = "ParaExcluir", DataNascimento = DateTime.Today.AddYears(-20) };
            var criada = await _pessoaService.CreateAsync(criarDto);
            Assert.NotNull(criada);

            await _pessoaService.DeleteAsync(criada.Id);
            var deletada = await _pessoaService.GetByIdAsync(criada.Id);
            Assert.Null(deletada);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
