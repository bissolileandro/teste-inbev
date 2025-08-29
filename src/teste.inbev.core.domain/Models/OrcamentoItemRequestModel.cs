using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teste.inbev.core.domain.Models
{
    public class OrcamentoItemRequestModel
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }         
        public string? Observacao { get; set; }
    }
}
