using FluentValidation;
using teste.inbev.core.domain.Models;

namespace teste.inbev.core.application.Validators
{
    public class ProdutoInserirValidator : AbstractValidator<ProdutoModel>
    {
        public ProdutoInserirValidator()
        {
            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("Produto deve conter descrição.");

            RuleFor(x => x.ValorUnitario)
                .GreaterThan(0).WithMessage("Valor do Produto deve ser maior que 0.");
        }
    }

    public class ProdutoAtualizarValidator : AbstractValidator<ProdutoModel>
    {
        public ProdutoAtualizarValidator()
        {
            Include(new ProdutoInserirValidator()); 
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id deve ser informado para atualização.");
        }
    }

    public class ProdutoDeletarValidator : AbstractValidator<ProdutoModel>
    {
        public ProdutoDeletarValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id deve ser informado para exclusão.");
        }
    }
}
