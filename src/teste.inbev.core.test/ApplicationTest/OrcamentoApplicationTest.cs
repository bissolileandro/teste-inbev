using Xunit;
using NSubstitute;
using FluentAssertions;
using AutoMapper;
using Bogus;
using System.Collections.Generic;
using System.Threading.Tasks;
using teste.inbev.core.application.Applications;
using teste.inbev.core.domain.Entities;
using teste.inbev.core.domain.Interface.Services;
using teste.inbev.core.domain.Models;
using teste.inbev.core.application.Validators;

namespace teste.inbev.core.test.ApplicationTest
{

    public class OrcamentoApplicationTests
    {
        private readonly IOrcamentoService _orcamentoService = Substitute.For<IOrcamentoService>();
        private readonly IProdutoService _produtoService = Substitute.For<IProdutoService>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();

        
        private readonly OrcamentoInserirValidator _inserirValidator = new OrcamentoInserirValidator();
        private readonly OrcamentoAtualizarValidator _atualizarValidator = new OrcamentoAtualizarValidator();
        private readonly OrcamentoItemInserirValidator _inserirItemValidator = new OrcamentoItemInserirValidator(); 
        private readonly OrcamentoDeletarValidator _deletarItemValidator = new OrcamentoDeletarValidator();
        private readonly OrcamentoApplication _application;

        public OrcamentoApplicationTests()
        {
            _application = new OrcamentoApplication(
                _orcamentoService,
                _mapper,
                _produtoService,
                _inserirValidator,      
                _atualizarValidator,
                _inserirItemValidator,  
                _deletarItemValidator
            );
        }
        
        private OrcamentoRequestModel GerarOrcamentoFake(int itens, int quantidadePorItem = 1)
        {
            var faker = new Faker();

            var listaItens = new List<OrcamentoItemRequestModel>();
            for (int i = 0; i < itens; i++)
            {
                listaItens.Add(new OrcamentoItemRequestModel
                {
                    ProdutoId = i + 1,
                    Quantidade = quantidadePorItem,
                });
            }

            return new OrcamentoRequestModel
            {
                Id = faker.Random.Int(1, 1000),
                IdentificacaoCliente = "123456",
                NomeCliente = "Teste 123",
                OrcamentoItems = listaItens
            };
        }

        [Theory]
        [InlineData(1, 1, 0)]   // sem desconto, total itens < 8
        [InlineData(2, 4, 0)]   // sem desconto, total itens = 8 mas quantidade por produto < 7
        [InlineData(2, 7, 0.10)]// desconto 10% aplicado, quantidade >= 7
        [InlineData(3, 15, 0.20)] // desconto 20% aplicado, quantidade >= 15
        public async Task InserirAsync_DeveAplicarDescontoCorreto(int itens, int quantidadePorItem, decimal descontoEsperado)
        {
            // Arrange
            var request = GerarOrcamentoFake(itens, quantidadePorItem);

            foreach (var item in request.OrcamentoItems)
            {
                var produto = new Produto
                {
                    Id = item.ProdutoId,
                    Descricao = $"Produto {item.ProdutoId}",
                    ValorUnitario = 10
                };

                _produtoService.ObterPorId(item.ProdutoId).Returns(Task.FromResult(produto));
            }
            
            var orcamentoEntity = new Orcamento
            {
                Id = request.Id,
                OrcamentoItems = new List<OrcamentoItem>(),
            };

            foreach (var item in request.OrcamentoItems)
            {
                orcamentoEntity.OrcamentoItems.Add(new OrcamentoItem
                {
                    ProdutoId = item.ProdutoId,
                    Quantidade = item.Quantidade,
                    Produto = _produtoService.ObterPorId(item.ProdutoId).Result
                });
            }

            _mapper.Map<Orcamento>(request).Returns(orcamentoEntity);
            _mapper.Map<OrcamentoResponseModel>(Arg.Any<Orcamento>()).Returns(ci => new OrcamentoResponseModel
            {
                Id = orcamentoEntity.Id,                
                SubTotal = orcamentoEntity.SubTotal,
                Total = orcamentoEntity.Total,
                DescontoPercentual = orcamentoEntity.DescontoPercentual,
                DescontoValor = orcamentoEntity.DescontoValor
            });

            _orcamentoService.InserirAsync(orcamentoEntity).Returns(Task.FromResult(orcamentoEntity));

            // Act
            var resultado = await _application.InserirAsync(request);

            // Assert
            var subtotalEsperado = itens * quantidadePorItem * 10; // 10 é ValorUnitario
            var descontoValorEsperado = subtotalEsperado * descontoEsperado;
            var totalEsperado = subtotalEsperado - descontoValorEsperado;

            resultado.SubTotal.Should().Be(subtotalEsperado);
            resultado.DescontoPercentual.Should().Be(descontoEsperado);
            resultado.DescontoValor.Should().Be(descontoValorEsperado);
            resultado.Total.Should().Be(totalEsperado);
        }

        [Fact]
        public async Task InserirAsync_DeveRetornarErro_QuandoItemForNulo()
        {
            // Arrange
            var request = new OrcamentoRequestModel
            {
                Id = 1,
                IdentificacaoCliente = "123",
                NomeCliente = "Teste",
                OrcamentoItems = null // inválido
            };

            // Act
            Func<Task> act = async () => await _application.InserirAsync(request);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Erro ao Inserir orçamento: A lista de itens não pode ser nula.\r\nO orcamento deve conter pelo menos um item.");
        }

        [Fact]
        public async Task InserirAsync_DeveRetornarErro_QuandoProdutoIdForNulo()
        {
            // Arrange
            var request = GerarOrcamentoFake(1);
            request.OrcamentoItems[0].ProdutoId = 0; // inválido

            // Act
            Func<Task> act = async () => await _application.InserirAsync(request);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Erro ao Inserir orçamento: ProdutoId deve ser diferente de zero para a inserção.");
        }

        [Fact]
        public async Task InserirAsync_DeveRetornarErro_QuandoQuantidadeDeProdutoExceder20()
        {
            // Arrange
            var request = GerarOrcamentoFake(1, 25); // quantidade > 20

            var produto = new Produto { Id = 1, Descricao = "Produto 1", ValorUnitario = 10 };
            _produtoService.ObterPorId(1).Returns(Task.FromResult(produto));

            var orcamentoEntity = _mapper.Map<Orcamento>(request);
            _orcamentoService.InserirAsync(orcamentoEntity).Returns(Task.FromResult(orcamentoEntity));

            // Act
            Func<Task> act = async () => await _application.InserirAsync(request);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage($"Erro ao Inserir orçamento: O produto {produto.Id} excedeu o limite de 20 unidades.");
        }

        [Fact]
        public async Task AtualizarAsync_DeveAtualizarComSucesso()
        {
            // Arrange            
            var request = GerarOrcamentoFake(2, 5); // 2 itens, 5 unidades cada

            // Configura os produtos mockados
            foreach (var item in request.OrcamentoItems)
            {
                var produto = new Produto
                {
                    Id = item.ProdutoId,
                    Descricao = $"Produto {item.ProdutoId}",
                    ValorUnitario = 10
                };
                _produtoService.ObterPorId(item.ProdutoId).Returns(Task.FromResult(produto));
            }

            // Cria a entidade Orcamento com itens e produtos
            var orcamentoEntity = new Orcamento
            {
                Id = request.Id,
                OrcamentoItems = request.OrcamentoItems.Select(i => new OrcamentoItem
                {
                    ProdutoId = i.ProdutoId,
                    Quantidade = i.Quantidade,
                    Produto = _produtoService.ObterPorId(i.ProdutoId).Result
                }).ToList()
            };

            // Mock do Mapper para Map<Orcamento>
            _mapper.Map<Orcamento>(request).Returns(orcamentoEntity);

            // Mock do serviço de atualização
            _orcamentoService.AtualizarAsync(orcamentoEntity).Returns(Task.FromResult(orcamentoEntity));

            // Mock do Mapper para Map<OrcamentoResponseModel> 
            // Aqui preenche manualmente os campos para simular o mapeamento real
            _mapper.Map<OrcamentoResponseModel>(Arg.Any<Orcamento>())
                .Returns(ci =>
                {
                    var o = ci.Arg<Orcamento>();
                    return new OrcamentoResponseModel
                    {
                        Id = o.Id,
                        SubTotal = o.SubTotal,
                        Total = o.Total,
                        DescontoPercentual = o.DescontoPercentual,
                        DescontoValor = o.DescontoValor
                    };
                });

            // Act
            var resultado = await _application.AtualizarAsync(request);

            // Assert
            resultado.Id.Should().Be(request.Id);
            resultado.SubTotal.Should().BeGreaterThan(0);
            resultado.Total.Should().Be(resultado.SubTotal - resultado.DescontoValor);
        }

        [Fact]
        public async Task AtualizarAsync_DeveRetornarErro_QuandoItemForNulo()
        {
            // Arrange
            var request = new OrcamentoRequestModel
            {
                Id = 1,
                IdentificacaoCliente = "123",
                NomeCliente = "Teste",
                OrcamentoItems = null // inválido
            };

            // Act
            Func<Task> act = async () => await _application.AtualizarAsync(request);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Erro ao atualizar orcamento: A lista de itens não pode ser nula.\r\nO orcamento deve conter pelo menos um item.");
        }

        [Fact]
        public async Task AtualizarAsync_DeveRetornarErro_QuandoProdutoIdForNulo()
        {
            // Arrange
            var request = GerarOrcamentoFake(1);
            request.OrcamentoItems[0].ProdutoId = 0; // inválido

            // Act
            Func<Task> act = async () => await _application.AtualizarAsync(request);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Erro ao atualizar orcamento: ProdutoId deve ser diferente de zero para a inserção.");
        }

        [Fact]
        public async Task AtualizarAsync_DeveRetornarErro_QuandoQuantidadeDeProdutoExceder20()
        {
            // Arrange
            var request = GerarOrcamentoFake(1, 25); // quantidade > 20

            var produto = new Produto { Id = 1, Descricao = "Produto 1", ValorUnitario = 10 };
            _produtoService.ObterPorId(1).Returns(Task.FromResult(produto));

            var orcamentoEntity = _mapper.Map<Orcamento>(request);
            _orcamentoService.AtualizarAsync(orcamentoEntity).Returns(Task.FromResult(orcamentoEntity));

            // Act
            Func<Task> act = async () => await _application.AtualizarAsync(request);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage($"Erro ao atualizar orcamento: O produto {produto.Id} excedeu o limite de 20 unidades.");
        }

        [Fact]
        public async Task DeleteAnsync_DeveDeletarComSucesso()
        {
            // Arrange
            var id = 1;
            var orcamentoEntity = new Orcamento { Id = id };
            var orcamentoResponse = new OrcamentoResponseModel { Id = id };

            _orcamentoService.ObterPorId(id).Returns(Task.FromResult(orcamentoEntity));
            _orcamentoService.DeleteAnsync(orcamentoEntity).Returns(Task.FromResult(orcamentoEntity));

            _mapper.Map<Orcamento>(orcamentoEntity).Returns(orcamentoEntity);
            _mapper.Map<OrcamentoResponseModel>(orcamentoEntity).Returns(orcamentoResponse);

            // Act
            var resultado = await _application.DeleteAnsync(id);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Id.Should().Be(id);
        }

        [Fact]
        public async Task DeleteAnsync_DeveRetornarErro_QuandoIdForInvalido()
        {
            // Arrange
            var id = 0; // inválido para validação

            // Act
            Func<Task> act = async () => await _application.DeleteAnsync(id);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Erro ao deletar orçamento: Id deve ser informado para exclusão.");
        }

        [Fact]
        public async Task DeleteAnsync_DeveRetornarErro_QuandoServicoFalhar()
        {
            // Arrange
            var id = 1;
            _orcamentoService.ObterPorId(id).Returns<Task<Orcamento>>(x => throw new Exception("Erro no banco"));

            // Act
            Func<Task> act = async () => await _application.DeleteAnsync(id);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Erro ao deletar orçamento: Erro no banco");
        }



    }
}