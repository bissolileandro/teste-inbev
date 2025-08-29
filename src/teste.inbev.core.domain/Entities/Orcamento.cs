using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teste.inbev.core.domain.Entities
{
    public class Orcamento : EntidadeBase
    {
        public string? IdentificacaoCliente { get; set; }
        public string? NomeCliente { get; set; }
        public virtual List<OrcamentoItem> OrcamentoItems { get; set; }
        public decimal DescontoPercentual { get; set; }
        public decimal DescontoValor { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public string? Observacao { get; set; }
    }
}
