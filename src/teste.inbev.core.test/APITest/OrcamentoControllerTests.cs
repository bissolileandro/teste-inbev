using Xunit;
using NSubstitute;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using teste.inbev.core.api.Controllers;
using teste.inbev.core.domain.Interface.Application;
using teste.inbev.core.domain.Models;

namespace teste.inbev.core.test.ControllerTest
{
    public class OrcamentoControllerTests
    {
        private readonly IOrcamentoApplication _orcamentoApplication = Substitute.For<IOrcamentoApplication>();
        private readonly ILogger<OrcamentoController> _logger = Substitute.For<ILogger<OrcamentoController>>();
        private readonly OrcamentoController _controller;

        public OrcamentoControllerTests()
        {
            _controller = new OrcamentoController(_orcamentoApplication, _logger);
        }

        [Fact]
        public async Task ObterPorId_DeveRetornarOk()
        {
            // Arrange
            var id = 1;
            var orcamento = new OrcamentoResponseModel { Id = id };
            _orcamentoApplication.ObterPorId(id).Returns(Task.FromResult(orcamento));

            // Act
            var result = await _controller.ObterPorId(id);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(orcamento);
        }

        [Fact]
        public async Task ObterPorId_DeveRetornarBadRequest_QuandoException()
        {
            // Arrange
            var id = 1;
            _orcamentoApplication.ObterPorId(id).Returns<Task<OrcamentoResponseModel>>(x => throw new Exception("Erro teste"));

            // Act
            var result = await _controller.ObterPorId(id);

            // Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest.Value.Should().Be("Erro: Erro teste");
        }

        [Fact]
        public async Task ListarTodos_DeveRetornarOk()
        {
            // Arrange
            var lista = new List<OrcamentoResponseModel>
            {
                new OrcamentoResponseModel { Id = 1 },
                new OrcamentoResponseModel { Id = 2 }
            };
            _orcamentoApplication.ListarTodosAsync().Returns(Task.FromResult(lista));

            // Act
            var result = await _controller.ListarTodos();

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(lista);
        }

        [Fact]
        public async Task Inserir_DeveRetornarCreatedAtAction()
        {
            // Arrange
            var model = new OrcamentoRequestModel { Id = 1 };
            var retorno = new OrcamentoResponseModel { Id = 1 };
            _orcamentoApplication.InserirAsync(model).Returns(Task.FromResult(retorno));

            // Act
            var result = await _controller.Inserir(model);

            // Assert
            var created = result as CreatedAtActionResult;
            created.Should().NotBeNull();
            created.ActionName.Should().Be(nameof(OrcamentoController.ObterPorId));
            created.Value.Should().BeEquivalentTo(retorno);
        }

        [Fact]
        public async Task Inserir_DeveRetornarBadRequest_QuandoException()
        {
            // Arrange
            var model = new OrcamentoRequestModel { Id = 1 };
            _orcamentoApplication.InserirAsync(model).Returns<Task<OrcamentoResponseModel>>(x => throw new Exception("Erro teste"));

            // Act
            var result = await _controller.Inserir(model);

            // Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest.Value.Should().Be("Erro: Erro teste");
        }

        [Fact]
        public async Task Atualizar_DeveRetornarNoContent()
        {
            // Arrange
            var model = new OrcamentoRequestModel { Id = 1 };
            _orcamentoApplication.AtualizarAsync(model).Returns(Task.FromResult(new OrcamentoResponseModel { Id = 1 }));

            // Act
            var result = await _controller.Atualizar(model);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Atualizar_DeveRetornarBadRequest_QuandoException()
        {
            // Arrange
            var model = new OrcamentoRequestModel { Id = 1 };
            _orcamentoApplication.AtualizarAsync(model).Returns<Task<OrcamentoResponseModel>>(x => throw new Exception("Erro teste"));

            // Act
            var result = await _controller.Atualizar(model);

            // Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest.Value.Should().Be("Erro: Erro teste");
        }

        [Fact]
        public async Task Deletar_DeveRetornarNoContent()
        {
            // Arrange
            var id = 1;
            _orcamentoApplication.DeleteAnsync(id).Returns(Task.FromResult(new OrcamentoResponseModel { Id = id }));

            // Act
            var result = await _controller.Deletar(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Deletar_DeveRetornarBadRequest_QuandoException()
        {
            // Arrange
            var id = 1;
            _orcamentoApplication.DeleteAnsync(id).Returns<Task<OrcamentoResponseModel>>(x => throw new Exception("Erro teste"));

            // Act
            var result = await _controller.Deletar(id);

            // Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest.Value.Should().Be("Erro: Erro teste");
        }
    }
}
