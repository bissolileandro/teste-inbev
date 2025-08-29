using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teste.inbev.core.domain.Entities
{
    public class Produto : EntidadeBase
    {        
        public string Descricao { get; set; }
        public decimal ValorUnitario { get; set; }        
        public bool Ativo { get; set; }        
    }
}
