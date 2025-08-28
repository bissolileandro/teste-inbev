using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teste.inbev.core.domain.Entities;

namespace teste.inbev.core.domain.Models
{
    public class OrcamentoItemResponseModel : BaseModel
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }        
        public virtual ProdutoModel Produto { get; set; }        
        public string Observacao { get; set; }
    }
}
