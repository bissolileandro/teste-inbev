using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teste.inbev.core.domain.Entities;

namespace teste.inbev.core.domain.Interface.Repositories
{
    public interface IOrcamentoRepository : IRepositoryBase<Orcamento>
    {
        Task<Orcamento> ObterPorId(int id);        
    }
}
