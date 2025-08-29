using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Testcontainers.MsSql;
using teste.inbev.core.application.Applications;
using teste.inbev.core.application.Configurations;
using teste.inbev.core.application.Validators;
using teste.inbev.core.data;
using teste.inbev.core.data.Context;
using teste.inbev.core.data.Repositories;
using teste.inbev.core.domain.Entities;
using teste.inbev.core.domain.Models;
using teste.inbev.core.service.Services;
using Xunit;

namespace teste.inbev.core.integration.test.ProdutoIntegrationTest
{
    public class ProdutoIntegrationTests : IAsyncLifetime
    {
        private readonly MsSqlContainer _sqlServerContainer;
        private InbevCoreContext _dbContext;
        private ProdutoApplication _produtoApplication;
        private IMapper _mapper;

        public ProdutoIntegrationTests()
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

            var produtoRepository = new ProdutoRepository(_dbContext);
            var produtoService = new ProdutoService(produtoRepository);

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();

            _produtoApplication = new ProdutoApplication(
                produtoService,
                _mapper,
                new ProdutoInserirValidator(),
                new ProdutoAtualizarValidator(),
                new ProdutoDeletarValidator()
            );
        }

        public async Task DisposeAsync()
        {
            await _sqlServerContainer.StopAsync();
        }

        private ProdutoModel GerarProdutoFake(string descricao = "Produto Teste", decimal valor = 50)
        {
            return new ProdutoModel
            {
                Id = 0,
                Descricao = descricao,
                ValorUnitario = valor
            };
        }

        [Fact]
        public async Task Inserir_Produto_DeveFuncionar()
        {
            // Arrange
            var request = GerarProdutoFake();

            // Act
            var inserido = await _produtoApplication.InserirAsync(request);

            // Assert
            Assert.NotNull(inserido);
            Assert.Equal(1, inserido.Id);
            Assert.Equal(request.Descricao, inserido.Descricao);
            Assert.Equal(request.ValorUnitario, inserido.ValorUnitario);
        }

        [Fact]
        public async Task Atualizar_Produto_DeveFuncionar()
        {
            // Arrange
            var request = GerarProdutoFake("Produto Original", 100);
            var inserido = await _produtoApplication.InserirAsync(request);
            Assert.NotNull(inserido);

            _dbContext.ChangeTracker.Clear();

            // Act
            request.Id = inserido.Id;
            request.Descricao = "Produto Atualizado";
            request.ValorUnitario = 200;

            var atualizado = await _produtoApplication.AtualizarAsync(request);

            // Assert
            Assert.NotNull(atualizado);
            Assert.Equal(inserido.Id, atualizado.Id);
            Assert.Equal("Produto Atualizado", atualizado.Descricao);
            Assert.Equal(200, atualizado.ValorUnitario);
        }

        [Fact]
        public async Task Deletar_Produto_DeveFuncionar()
        {
            // Arrange
            var request = GerarProdutoFake("Produto para Deletar", 30);
            var inserido = await _produtoApplication.InserirAsync(request);
            Assert.NotNull(inserido);

            _dbContext.ChangeTracker.Clear();

            // Act
            var deletado = await _produtoApplication.DeleteAnsync(inserido.Id);

            // Assert
            Assert.NotNull(deletado);
            var fromDb = await _dbContext.Set<Produto>().FindAsync(inserido.Id);
            Assert.Null(fromDb);
        }
    }
}
