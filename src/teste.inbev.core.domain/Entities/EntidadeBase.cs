using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teste.inbev.core.domain.Entities
{
    public class EntidadeBase
    {
        public int Id { get; set; }
        public DateTime DataDeCriacao { get; set; }
        public DateTime DataDeAtualizacao { get; set; }

    }
}
