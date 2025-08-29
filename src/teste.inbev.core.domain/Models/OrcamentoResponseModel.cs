using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teste.inbev.core.domain.Entities;

namespace teste.inbev.core.domain.Models
{
    public class OrcamentoResponseModel
    {
        public int Id { get; set; }        
        public string? IdentificacaoCliente { get; set; }
        public string? NomeCliente { get; set; }
        public decimal DescontoPercentual { get; set; }
        public decimal DescontoValor { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public DateTime DataDeCriacao { get; set; }
        public DateTime DataDeAtualizacao { get; set; }
        public string Observacao { get; set; }
        public virtual List<OrcamentoItemResponseModel> OrcamentoItems { get; set; }
    }
}
