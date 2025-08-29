using AutoMapper;
using FluentValidation;
using teste.inbev.core.application.Validators;
using teste.inbev.core.domain.Entities;
using teste.inbev.core.domain.Interface.Application;
using teste.inbev.core.domain.Interface.Services;
using teste.inbev.core.domain.Models;

namespace teste.inbev.core.application.Applications
{
    public class ProdutoApplication : IProdutoApplication
    {
        public readonly IProdutoService produtoService;
        private readonly IMapper mapper;
        private readonly IValidator<ProdutoModel> inserirValidator;
        private readonly IValidator<ProdutoModel> atualizarValidator;
        private readonly IValidator<ProdutoModel> deletarValidator;
        public ProdutoApplication(IProdutoService produtoService, IMapper mapper, 
            ProdutoInserirValidator inserirValidator,ProdutoAtualizarValidator atualizarValidator,
            ProdutoDeletarValidator deletarValidator)            
        {
            this.produtoService = produtoService;
            this.mapper = mapper;
            this.inserirValidator = inserirValidator;
            this.atualizarValidator = atualizarValidator;
            this.deletarValidator = deletarValidator;
        }        

        public async Task<List<ProdutoModel>> ListarTodosAsync()
        {
            try
            {
                var listProdutos = mapper.Map<List<ProdutoModel>>(await produtoService.ListarTodosAsync());
                return listProdutos;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao listar os porodutos {e.Message}");
            }
        }
        public async Task<ProdutoModel> ObterPorId(int id)
        {
            try
            {
                var produto = mapper.Map<ProdutoModel>(await produtoService.ObterPorId(id));
                return produto;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao buscar produto: {e.Message}");
            }

        }

        public async Task<ProdutoModel> InserirAsync(ProdutoModel produtoModel)
        {
            try
            {
                var result = inserirValidator.Validate(produtoModel);

                if (!result.IsValid)
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.ErrorMessage)));
                var produto = mapper.Map<Produto>(produtoModel);
                var retorno = mapper.Map<ProdutoModel>(await produtoService.InserirAsync(produto));
                return retorno;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao Inserir produto: {e.Message}");
            }

        }

        public async Task<ProdutoModel> AtualizarAsync(ProdutoModel produtoModel)
        {
            try
            {
                var result = atualizarValidator.Validate(produtoModel);

                if (!result.IsValid)
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.ErrorMessage)));
                var produto = mapper.Map<Produto>(produtoModel);
                var retorno = mapper.Map<ProdutoModel>(await produtoService.AtualizarAsync(produto));
                return retorno;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao atualizar produto: {e.Message}");
            }

        }

        public async Task<ProdutoModel> DeleteAnsync(int id)
        {
            try
            {
                var result = deletarValidator.Validate(new ProdutoModel { Id = id });

                if (!result.IsValid)
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.ErrorMessage)));
                var produto = mapper.Map<Produto>(await produtoService.ObterPorId(id));
                return mapper.Map<ProdutoModel>(await produtoService.DeleteAnsync(produto));
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao deletar produto: {e.Message}");
            }

        }
    }
}
