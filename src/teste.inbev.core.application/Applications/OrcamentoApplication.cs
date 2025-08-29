using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teste.inbev.core.application.Validators;
using teste.inbev.core.domain.Entities;
using teste.inbev.core.domain.Interface.Application;
using teste.inbev.core.domain.Interface.Services;
using teste.inbev.core.domain.Models;
using teste.inbev.core.service.Services;

namespace teste.inbev.core.application.Applications
{
    public class OrcamentoApplication : IOrcamentoApplication
    {
        private readonly IOrcamentoService orcamentoService;        
        private readonly IProdutoService produtoService;
        private readonly IMapper mapper;
        private readonly IValidator<OrcamentoRequestModel> inserirOrcamentoValidator;
        private readonly IValidator<OrcamentoRequestModel> atualizarOrcamentoValidator;
        private readonly IValidator<OrcamentoRequestModel> deletarOrcamentoValidator;
        private readonly IValidator<List<OrcamentoItemRequestModel>> inserirOrcamentoItemValidator;
        public OrcamentoApplication(IOrcamentoService orcamentoService,
            IMapper mapper, IProdutoService produtoService,
            OrcamentoInserirValidator inserirOrcamentoValidator,
            OrcamentoAtualizarValidator atualizarOrcamentoValidator,
            OrcamentoItemInserirValidator inserirOrcamentoItemValidator,
            OrcamentoDeletarValidator deletarOrcamentoValidator)
        {
            
            this.orcamentoService = orcamentoService;
            this.mapper = mapper;
            this.inserirOrcamentoValidator = inserirOrcamentoValidator;
            this.inserirOrcamentoItemValidator = inserirOrcamentoItemValidator;
            this.deletarOrcamentoValidator = deletarOrcamentoValidator;
            this.atualizarOrcamentoValidator = atualizarOrcamentoValidator;
            this.produtoService = produtoService;
        }
        public async Task<OrcamentoResponseModel> AtualizarAsync(OrcamentoRequestModel orcamentoModel)
        {
            try
            {

                var resultOrcamento = atualizarOrcamentoValidator.Validate(orcamentoModel);

                if (!resultOrcamento.IsValid)
                    throw new Exception(string.Join(Environment.NewLine, resultOrcamento.Errors.Select(e => e.ErrorMessage)));

                var resultOrcamentoItems = inserirOrcamentoItemValidator.Validate(orcamentoModel.OrcamentoItems);

                if (!resultOrcamentoItems.IsValid)
                    throw new Exception(string.Join(Environment.NewLine, resultOrcamentoItems.Errors.Select(e => e.ErrorMessage)));

                var orcamento = mapper.Map<Orcamento>(orcamentoModel);
                orcamento = CalcularOrcamento(orcamento);
                orcamento.DataDeAtualizacao = DateTime.Now;
                var retorno = mapper.Map<OrcamentoResponseModel>(await orcamentoService.AtualizarAsync(orcamento));
                return retorno;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao atualizar orcamento: {e.Message}");
            }
        }

        public async Task<OrcamentoResponseModel> DeleteAnsync(int id)
        {
            try
            {
                var result = deletarOrcamentoValidator.Validate(new OrcamentoRequestModel { Id = id });

                if (!result.IsValid)
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.ErrorMessage)));

                var orcamento = mapper.Map<Orcamento>(await orcamentoService.ObterPorId(id));                
                return mapper.Map<OrcamentoResponseModel>(await orcamentoService.DeleteAnsync(orcamento));
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao deletar orçamento: {e.Message}");
            }
        }

        public async Task<OrcamentoResponseModel> InserirAsync(OrcamentoRequestModel orcamentoModel)
        {
            try
            {
                var resultOrcamento = inserirOrcamentoValidator.Validate(orcamentoModel);

                if (!resultOrcamento.IsValid)
                    throw new Exception(string.Join(Environment.NewLine, resultOrcamento.Errors.Select(e => e.ErrorMessage)));

                var resultOrcamentoItems = inserirOrcamentoItemValidator.Validate(orcamentoModel.OrcamentoItems);

                if (!resultOrcamentoItems.IsValid)
                    throw new Exception(string.Join(Environment.NewLine, resultOrcamentoItems.Errors.Select(e => e.ErrorMessage)));

                var orcamento = mapper.Map<Orcamento>(orcamentoModel);
                orcamento = CalcularOrcamento(orcamento);
                orcamento.DataDeCriacao = DateTime.Now;
                orcamento.DataDeAtualizacao = DateTime.Now;
                var retorno = mapper.Map<OrcamentoResponseModel>(await orcamentoService.InserirAsync(orcamento));
                return retorno;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao Inserir orçamento: {e.Message}");
            }
        }

        public async Task<List<OrcamentoResponseModel>> ListarTodosAsync()
        {
            try
            {
                var listOrcamentos = mapper.Map<List<OrcamentoResponseModel>>(await orcamentoService.ListarTodosAsync());

                return listOrcamentos;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao listar os orçamentos {e.Message}");
            }
        }

        public async Task<OrcamentoResponseModel> ObterPorId(int id)
        {
            try
            {
                var orcamento = mapper.Map<OrcamentoResponseModel>(await orcamentoService.ObterPorId(id));
                return orcamento;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao buscar orçamento: {e.Message}");
            }
        }        

        private Orcamento CalcularOrcamento(Orcamento orcamento)
        {            
            
            var totalQuantidade = orcamento.OrcamentoItems.Sum(i => i.Quantidade);
            decimal totalPorProduto = 0;
            decimal orcamentoTotal = 0;
            decimal descontoPercentual = 0;                     

           
            var descontosPorProduto = orcamento.OrcamentoItems
                .GroupBy(i => i.ProdutoId)
                .Select(g => new OrcamentoItem
                {
                    ProdutoId = g.Key,
                    Quantidade = g.Sum(x => x.Quantidade),      
                    Produto = produtoService.ObterPorId(g.Key).Result,
                })
                .ToList();

            foreach (var item in descontosPorProduto)
            {
                if (totalQuantidade >= 8)
                {
                    if (item.Quantidade >= 15 && descontoPercentual == 0)
                    {
                        descontoPercentual = 0.20m;
                        orcamento.Observacao += $" Desconto de 20% aplicado no produto: {item.ProdutoId} - {item.Produto.Descricao}";
                    }
                    else if (item.Quantidade >= 7 && descontoPercentual == 0)
                    {
                        descontoPercentual = 0.10m;
                        orcamento.Observacao += $" Desconto de 10% aplicado no produto: {item.ProdutoId} - {item.Produto.Descricao}";

                    }
                }             
                totalPorProduto = item.Quantidade * item.Produto.ValorUnitario;
                orcamentoTotal = orcamentoTotal + totalPorProduto;
            }

            orcamento.SubTotal = orcamentoTotal;
            orcamento.DescontoPercentual = descontoPercentual;
            orcamento.DescontoValor = orcamento.SubTotal * orcamento.DescontoPercentual;
            orcamento.Total = orcamento.SubTotal - orcamento.DescontoValor;

            

            return orcamento;
            
        }
    }
}
