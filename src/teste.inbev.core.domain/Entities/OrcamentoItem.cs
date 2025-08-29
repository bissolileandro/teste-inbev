using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teste.inbev.core.domain.Entities
{
    public class OrcamentoItem : EntidadeBase
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }           
        public decimal TotalItem { get; set; }
        public virtual Produto Produto { get; set; }
        public int OrcamentoId { get; set; } 
        public virtual Orcamento Orcamento { get; set; }
        public string? Observacao { get; set; }
    }
}
