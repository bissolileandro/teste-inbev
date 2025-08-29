using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teste.inbev.core.domain.Entities;

namespace teste.inbev.core.domain.Interface.Repositories
{
    public interface IOrcamentoItemRepository : IRepositoryBase<OrcamentoItem>
    {
        Task<OrcamentoItem> ObterPorId(int id);        
        Task<List<OrcamentoItem>> ObterPorOrcamentoId(int orcamentoId);
    }
}
