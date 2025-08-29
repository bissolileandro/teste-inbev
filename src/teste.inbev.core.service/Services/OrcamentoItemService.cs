using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teste.inbev.core.domain.Entities;
using teste.inbev.core.domain.Interface.Repositories;
using teste.inbev.core.domain.Interface.Services;

namespace teste.inbev.core.service.Services
{
    public class OrcamentoItemService : ServiceBase<OrcamentoItem>, IOrcamentoItemService
    {
        private readonly IOrcamentoItemRepository orcamentoItemRepository;
        public OrcamentoItemService(IOrcamentoItemRepository orcamentoItemRepository)
            :base(orcamentoItemRepository)
        {
            this.orcamentoItemRepository = orcamentoItemRepository;
        }
        public async Task<OrcamentoItem> ObterPorId(int id)
        {
            return await orcamentoItemRepository.ObterPorId(id);
        }

        public async Task<List<OrcamentoItem>> ObterPorOrcamentoId(int orcamentoId)
        {
            return await orcamentoItemRepository.ObterPorOrcamentoId(orcamentoId);
        }
    }
}
