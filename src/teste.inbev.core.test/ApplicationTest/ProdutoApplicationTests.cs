using AutoMapper;
using Bogus;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using teste.inbev.core.application.Applications;
using teste.inbev.core.application.Validators;
using teste.inbev.core.domain.Entities;
using teste.inbev.core.domain.Interface.Services;
using teste.inbev.core.domain.Models;
using Xunit;

namespace teste.inbev.core.test.ApplicationTest
{
    public class ProdutoApplicationTests
    {
        private readonly IProdutoService _produtoService = Substitute.For<IProdutoService>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();

        private readonly ProdutoInserirValidator _inserirValidator = new ProdutoInserirValidator();
        private readonly ProdutoAtualizarValidator _atualizarValidator = new ProdutoAtualizarValidator();
        private readonly ProdutoDeletarValidator _deletarValidator = new ProdutoDeletarValidator();

        private readonly ProdutoApplication _application;

        public ProdutoApplicationTests()
        {
            _application = new ProdutoApplication(
                _produtoService,
                _mapper,
                _inserirValidator,
                _atualizarValidator,
                _deletarValidator
            );
        }

        private ProdutoModel GerarProdutoFake()
        {
            var faker = new Faker();
            return new ProdutoModel
            {
                Id = faker.Random.Int(1, 1000),
                Descricao = faker.Commerce.ProductName(),
                ValorUnitario = faker.Random.Decimal(1, 100)
            };
        }

        [Fact]
        public async Task InserirAsync_DeveInserirComSucesso()
        {
            // Arrange
            var produto = GerarProdutoFake();
            var produtoEntity = new Produto { Id = produto.Id, Descricao = produto.Descricao, ValorUnitario = produto.ValorUnitario };

            _mapper.Map<Produto>(produto).Returns(produtoEntity);
            _produtoService.InserirAsync(produtoEntity).Returns(Task.FromResult(produtoEntity));
            _mapper.Map<ProdutoModel>(produtoEntity).Returns(produto);

            // Act
            var resultado = await _application.InserirAsync(produto);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Id.Should().Be(produto.Id);
        }

        [Fact]
        public async Task AtualizarAsync_DeveAtualizarComSucesso()
        {
            // Arrange
            var produto = GerarProdutoFake();
            var produtoEntity = new Produto { Id = produto.Id, Descricao = produto.Descricao, ValorUnitario = produto.ValorUnitario };

            _mapper.Map<Produto>(produto).Returns(produtoEntity);
            _produtoService.AtualizarAsync(produtoEntity).Returns(Task.FromResult(produtoEntity));
            _mapper.Map<ProdutoModel>(produtoEntity).Returns(produto);

            // Act
            var resultado = await _application.AtualizarAsync(produto);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Id.Should().Be(produto.Id);
        }

        [Fact]
        public async Task DeleteAnsync_DeveDeletarComSucesso()
        {
            // Arrange
            var produto = GerarProdutoFake();
            var produtoEntity = new Produto { Id = produto.Id, Descricao = produto.Descricao, ValorUnitario = produto.ValorUnitario };

            _produtoService.ObterPorId(produto.Id).Returns(Task.FromResult(produtoEntity));
            _produtoService.DeleteAnsync(produtoEntity).Returns(Task.FromResult(produtoEntity));

            _mapper.Map<Produto>(produtoEntity).Returns(produtoEntity);
            _mapper.Map<ProdutoModel>(produtoEntity).Returns(produto);

            // Act
            var resultado = await _application.DeleteAnsync(produto.Id);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Id.Should().Be(produto.Id);
        }

        [Fact]
        public async Task InserirAsync_DeveRetornarErro_QuandoDescricaoNula()
        {
            // Arrange
            var produto = new ProdutoModel { Id = 0, Descricao = null, ValorUnitario = 10 };

            // Act
            Func<Task> act = async () => await _application.InserirAsync(produto);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Erro ao Inserir produto: Produto deve conter descrição.");
        }

        [Fact]
        public async Task AtualizarAsync_DeveRetornarErro_QuandoIdZero()
        {
            // Arrange
            var produto = new ProdutoModel { Id = 0, Descricao = "Teste", ValorUnitario = 10 };

            // Act
            Func<Task> act = async () => await _application.AtualizarAsync(produto);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("*Id deve ser informado*");
        }

        [Fact]
        public async Task DeleteAnsync_DeveRetornarErro_QuandoIdZero()
        {
            // Arrange
            var id = 0;

            // Act
            Func<Task> act = async () => await _application.DeleteAnsync(id);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("*Id deve ser informado*");
        }
    }
}
