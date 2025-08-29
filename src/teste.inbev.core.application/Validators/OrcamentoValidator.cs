using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teste.inbev.core.domain.Models;

namespace teste.inbev.core.application.Validators
{
    public class OrcamentoInserirValidator : AbstractValidator<OrcamentoRequestModel>
    {
        public OrcamentoInserirValidator()
        {
            RuleFor(x => x.OrcamentoItems)
                .NotNull().WithMessage("A lista de itens não pode ser nula.")
                .NotEmpty().WithMessage("O orcamento deve conter pelo menos um item.");

            RuleFor(x => x.IdentificacaoCliente)
                .NotNull().NotEmpty().WithMessage("Deve conter Identificação do cliente no orçamento.");

            RuleFor(x => x.NomeCliente)
                .NotNull().NotEmpty().WithMessage("Deve conter nome do cliente no orçamento.");
        }
    }

    public class OrcamentoAtualizarValidator : AbstractValidator<OrcamentoRequestModel>
    {
        public OrcamentoAtualizarValidator()
        {
            Include(new OrcamentoInserirValidator());
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id deve ser informado para atualização.");
        }
    }

    public class OrcamentoDeletarValidator : AbstractValidator<OrcamentoRequestModel>
    {
        public OrcamentoDeletarValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id deve ser informado para exclusão.");
        }
    }
}
