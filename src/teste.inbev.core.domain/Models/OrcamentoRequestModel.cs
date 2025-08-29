using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teste.inbev.core.domain.Models
{
    public class OrcamentoRequestModel
    {
        public int Id { get; set; }
        public string? IdentificacaoCliente { get; set; }
        public string? NomeCliente { get; set; }
        public virtual List<OrcamentoItemRequestModel> OrcamentoItems { get; set; }        
        public string Observacao { get; set; }
    }
}
