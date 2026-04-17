using System;
using System.Threading.Tasks;
using MinhasFinancas.Application.DTOs;
using MinhasFinancas.Application.Services;
using MinhasFinancas.Domain.Entities;
using MinhasFinancas.Domain.Interfaces;
using Moq;
using Xunit;

namespace MinhasFinancas.UnitTests;

public class TransacaoServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly TransacaoService _service;

    public TransacaoServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _service = new TransacaoService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task CreateAsync_QuandoPessoaMenorDeIdadeETipoReceita_DeveLancarExcecao()
    {
        var pessoaMenor = new Pessoa
        {
            Id = Guid.NewGuid(),
            Nome = "João Menor",
            DataNascimento = DateTime.Today.AddYears(-16)
        };
        var categoria = new Categoria
        {
            Id = Guid.NewGuid(),
            Descricao = "Salário",
            Finalidade = Categoria.EFinalidade.Receita
        };
        var dto = new CreateTransacaoDto
        {
            Descricao = "Salário",
            Valor = 1000,
            Data = DateTime.Today,
            Tipo = Transacao.ETipo.Receita,
            CategoriaId = categoria.Id,
            PessoaId = pessoaMenor.Id
        };

        _unitOfWorkMock.Setup(u => u.Pessoas.GetByIdAsync(pessoaMenor.Id))
            .ReturnsAsync(pessoaMenor);
        _unitOfWorkMock.Setup(u => u.Categorias.GetByIdAsync(categoria.Id))
            .ReturnsAsync(categoria);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_QuandoPessoaMaiorDeIdadeETipoReceita_DeveCriarComSucesso()
    {
        var pessoaMaior = new Pessoa
        {
            Id = Guid.NewGuid(),
            Nome = "Maria Maior",
            DataNascimento = DateTime.Today.AddYears(-25)
        };
        var categoria = new Categoria
        {
            Id = Guid.NewGuid(),
            Descricao = "Salário",
            Finalidade = Categoria.EFinalidade.Receita
        };
        var dto = new CreateTransacaoDto
        {
            Descricao = "Salário",
            Valor = 2000,
            Data = DateTime.Today,
            Tipo = Transacao.ETipo.Receita,
            CategoriaId = categoria.Id,
            PessoaId = pessoaMaior.Id
        };

        _unitOfWorkMock.Setup(u => u.Pessoas.GetByIdAsync(pessoaMaior.Id))
            .ReturnsAsync(pessoaMaior);
        _unitOfWorkMock.Setup(u => u.Categorias.GetByIdAsync(categoria.Id))
            .ReturnsAsync(categoria);
        _unitOfWorkMock.Setup(u => u.Transacoes.AddAsync(It.IsAny<Transacao>()))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(1);

        var result = await _service.CreateAsync(dto);

        Assert.NotNull(result);
        Assert.Equal(dto.Valor, result.Valor);
        _unitOfWorkMock.Verify(u => u.Transacoes.AddAsync(It.IsAny<Transacao>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_QuandoTipoTransacaoIncompativelComFinalidadeCategoria_DeveLancarExcecao()
    {
        var pessoa = new Pessoa
        {
            Id = Guid.NewGuid(),
            Nome = "João Silva",
            DataNascimento = DateTime.Today.AddYears(-30)
        };
        var categoriaApenasDespesa = new Categoria
        {
            Id = Guid.NewGuid(),
            Descricao = "Lazer",
            Finalidade = Categoria.EFinalidade.Despesa
        };
        var dto = new CreateTransacaoDto
        {
            Descricao = "Salário",
            Valor = 1000,
            Data = DateTime.Today,
            Tipo = Transacao.ETipo.Receita,
            CategoriaId = categoriaApenasDespesa.Id,
            PessoaId = pessoa.Id
        };

        _unitOfWorkMock.Setup(u => u.Pessoas.GetByIdAsync(pessoa.Id))
            .ReturnsAsync(pessoa);
        _unitOfWorkMock.Setup(u => u.Categorias.GetByIdAsync(categoriaApenasDespesa.Id))
            .ReturnsAsync(categoriaApenasDespesa);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));
    }
}
