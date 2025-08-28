using AutoMapper;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using System;
using System.Linq;
using System.Threading.Tasks;
using Testcontainers.MsSql;
using teste.inbev.core.application.Applications;
using teste.inbev.core.application.Configurations;
using teste.inbev.core.application.Validators;
using teste.inbev.core.data;
using teste.inbev.core.data.Context;
using teste.inbev.core.domain.Entities;
using teste.inbev.core.domain.Interface.Services;
using teste.inbev.core.domain.Models;
using teste.inbev.core.service.Services;
using Xunit;

namespace teste.inbev.core.integration.test.OrcamentoIntegrationTest
{
    public class OrcamentoIntegrationTests : IAsyncLifetime
    {
        private readonly MsSqlContainer _sqlServerContainer;
        private InbevCoreContext _dbContext;
        private OrcamentoApplication _orcamentoApplication;
        private IProdutoService _produtoService = Substitute.For<IProdutoService>();
        private IMapper _mapper;

        public OrcamentoIntegrationTests()
        {
            _sqlServerContainer = new MsSqlBuilder()                
                .WithCleanUp(true)
                .Build();
        }

        public async Task InitializeAsync()
        {
            await _sqlServerContainer.StartAsync();

            var options = new DbContextOptionsBuilder<InbevCoreContext>()
                .UseSqlServer(_sqlServerContainer.GetConnectionString())
                .Options;

            _dbContext = new InbevCoreContext(options);
            await _dbContext.Database.EnsureCreatedAsync();

            var orcamentoRepository = new teste.inbev.core.data.Repositories.OrcamentoRepository(_dbContext);
            var orcamentoService = new OrcamentoService(orcamentoRepository);

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>(); // seu Profile que mapeia Orcamento <-> OrcamentoResponseModel
                                                    // Se tiver outros Profiles, adicione aqui
            });
            
            _mapper = mapperConfig.CreateMapper();

            _orcamentoApplication = new OrcamentoApplication(
                orcamentoService,
                _mapper, // pode mockar ou usar mapper real
                _produtoService,
                new OrcamentoInserirValidator(),
                new OrcamentoAtualizarValidator(),
                new OrcamentoItemInserirValidator(),
                new OrcamentoDeletarValidator()
            );
        }

        public async Task DisposeAsync()
        {
            await _sqlServerContainer.StopAsync();
        }        

        private OrcamentoRequestModel GerarOrcamentoInsertFake(int itens = 2, int quantidadePorItem = 5)
        {
            var faker = new Faker();
            var listaItens = Enumerable.Range(1, itens).Select(i => new OrcamentoItemRequestModel
            {
                ProdutoId = i,
                Quantidade = quantidadePorItem
            }).ToList();

            return new OrcamentoRequestModel
            {
                Id = 0,
                IdentificacaoCliente = "123456",
                NomeCliente = "Cliente Teste",
                OrcamentoItems = listaItens
            };
        }

        private async Task ArrangeProduto()
        {
            var produtos = new[]
                        {
                new Produto { Id = 0, Descricao = "Produto 1", ValorUnitario = 10 },
                new Produto { Id = 0, Descricao = "Produto 2", ValorUnitario = 20 }
            };
            _dbContext.Set<Produto>().AddRange(produtos);
            await _dbContext.SaveChangesAsync();
        }

        [Fact]       
        public async Task Inserir_Orcamento_DeveFuncionar()
        {
            // Arrange
            await ArrangeProduto();
            var request = GerarOrcamentoInsertFake();
            foreach (var item in request.OrcamentoItems)
            {
                _produtoService.ObterPorId(item.ProdutoId).Returns(Task.FromResult(new Produto
                {
                    Id = item.ProdutoId,
                    Descricao = $"Produto {item.ProdutoId}",
                    ValorUnitario = 10
                }));
            }

            // Act
            var inserido = await _orcamentoApplication.InserirAsync(request);

            // Assert
            Assert.NotNull(inserido);
            Assert.Equal(1, inserido.Id);
            Assert.True(inserido.SubTotal > 0);
        }

        [Fact]
        public async Task Atualizar_Orcamento_DeveFuncionar()
        {
            // Arrange            
            await ArrangeProduto();
            var request = GerarOrcamentoInsertFake();
            foreach (var item in request.OrcamentoItems)
            {
                _produtoService.ObterPorId(item.ProdutoId).Returns(Task.FromResult(new Produto
                {
                    Id = item.ProdutoId,
                    Descricao = $"Produto {item.ProdutoId}",
                    ValorUnitario = 10
                }));
            }

            // Primeiro inserimos para existir no banco
            var inserido = await _orcamentoApplication.InserirAsync(request);
            // Assert
            Assert.NotNull(inserido);
            _dbContext.ChangeTracker.Clear();

            // Act - Atualizar
            request.OrcamentoItems[0].Quantidade = 15;
            request.Id = inserido.Id;
            var atualizado = await _orcamentoApplication.AtualizarAsync(request);

            // Assert
            Assert.NotNull(atualizado);
            Assert.True(atualizado.SubTotal > inserido.SubTotal); // SubTotal deve aumentar
        }       

        [Fact]
        public async Task Deletar_Orcamento_DeveFuncionar()
        {
            // Arrange
            await ArrangeProduto();
            var request = GerarOrcamentoInsertFake();
            foreach (var item in request.OrcamentoItems)
            {
                _produtoService.ObterPorId(item.ProdutoId).Returns(Task.FromResult(new Produto
                {
                    Id = item.ProdutoId,
                    Descricao = $"Produto {item.ProdutoId}",
                    ValorUnitario = 10
                }));
            }

            // Primeiro inserimos para existir no banco
            var inserido = await _orcamentoApplication.InserirAsync(request);
            // Assert
            Assert.NotNull(inserido);
            _dbContext.ChangeTracker.Clear();

            // Act - Deletar
            var deletado = await _orcamentoApplication.DeleteAnsync(inserido.Id);

            // Assert
            Assert.NotNull(deletado);
            var fromDb = await _dbContext.Set<Orcamento>().FindAsync(request.Id);
            Assert.Null(fromDb); // Confirma que foi deletado do banco
        }

    }
}
