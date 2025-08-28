using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teste.inbev.core.domain.Models;

namespace teste.inbev.core.application.Validators
{
    public class OrcamentoItemInserirValidator : AbstractValidator<List<OrcamentoItemRequestModel>>
    {
        public OrcamentoItemInserirValidator()
        {
            

            RuleFor(x => x)
            .NotNull().WithMessage("A lista de itens não pode ser nula.")
            .NotEmpty().WithMessage("O orçamento deve conter pelo menos um item.");
            
            RuleForEach(x => x).ChildRules(item =>
            {
                item.RuleFor(i => i.ProdutoId)
                .NotNull().WithMessage("ProdutoId deve ser informada para a inserção.")
                .GreaterThan(0).WithMessage("ProdutoId deve ser diferente de zero para a inserção.");

                item.RuleFor(i => i.Quantidade)
                .NotNull().WithMessage("Quantidade deve ser informada para a inserção.")
                .GreaterThan(0).WithMessage("Quantidade deve ser maior que zero para a inserção.");
            });

            RuleFor(x => x).Custom((lista, context) =>
            {
                var produtosComMaisDe20 = lista
                    .GroupBy(i => i.ProdutoId)
                    .Where(g => g.Sum(i => i.Quantidade) > 20)
                    .ToList();

                foreach (var produto in produtosComMaisDe20)
                {
                    context.AddFailure($"O produto {produto.Key} excedeu o limite de 20 unidades.");
                }
            });
        }
    }
}
