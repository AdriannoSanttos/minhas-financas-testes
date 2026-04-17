using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MinhasFinancas.Domain.Entities;
using MinhasFinancas.Infrastructure.Data;
using MinhasFinancas.Infrastructure;
using Xunit;

namespace MinhasFinancas.IntegrationTests
{
    public class CascataIntegrationTest : IDisposable
    {
        private readonly MinhasFinancasDbContext _context;
        private readonly UnitOfWork _unitOfWork;

        public CascataIntegrationTest()
        {
            var options = new DbContextOptionsBuilder<MinhasFinancasDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            _context = new MinhasFinancasDbContext(options);
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();
            _unitOfWork = new UnitOfWork(_context);
        }

        [Fact]
        public async Task ExcluirPessoa_DeveRemoverTransacoes()
        {
            var pessoa = new Pessoa { Nome = "Teste", DataNascimento = DateTime.Today.AddYears(-20) };
            await _context.Pessoas.AddAsync(pessoa);
            await _unitOfWork.SaveChangesAsync();

            var categoria = new Categoria { Descricao = "Cat", Finalidade = Categoria.EFinalidade.Ambas };
            await _context.Categorias.AddAsync(categoria);
            await _unitOfWork.SaveChangesAsync();

            var transacao = new Transacao
            {
                Descricao = "Compra",
                Valor = 100,
                Data = DateTime.Today,
                Tipo = Transacao.ETipo.Despesa
            };
            SetPrivateProperty(transacao, "PessoaId", pessoa.Id);
            SetPrivateProperty(transacao, "CategoriaId", categoria.Id);

            await _context.Transacoes.AddAsync(transacao);
            await _unitOfWork.SaveChangesAsync();

            _context.Pessoas.Remove(pessoa);
            await _unitOfWork.SaveChangesAsync();

            var existe = await _context.Transacoes.FindAsync(transacao.Id);
            Assert.Null(existe);
        }

        private void SetPrivateProperty(object obj, string propertyName, object value)
        {
            var prop = obj.GetType().GetProperty(propertyName);
            if (prop == null) throw new ArgumentException($"Property {propertyName} not found");
            prop.SetValue(obj, value);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
