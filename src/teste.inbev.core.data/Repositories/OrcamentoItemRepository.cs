using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teste.inbev.core.data.Context;
using teste.inbev.core.domain.Entities;
using teste.inbev.core.domain.Interface.Repositories;

namespace teste.inbev.core.data.Repositories
{
    public class OrcamentoItemRepository : RepositoryBase<OrcamentoItem>, IOrcamentoItemRepository
    {
        public OrcamentoItemRepository(InbevCoreContext context)
            : base(context)
        {

        }
        public async Task<OrcamentoItem> ObterPorId(int id)
        {
            try
            {
                var orcamentoItem = await db.Set<OrcamentoItem>()
                    .Include(c => c.Orcamento)
                    .Include(c => c.Produto)
                    .FirstOrDefaultAsync(i => i.Id == id);

                return orcamentoItem;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao Obter Item do Orçamento : {e.Message}");
            }
        }

        public async Task<List<OrcamentoItem>> ObterPorOrcamentoId(int orcamentoId)
        {
            try
            {
                var orcamentoItem = await db.Set<OrcamentoItem>()
                    .Include(c => c.Orcamento)
                    .Include(c => c.Produto)
                    .Where(i => i.OrcamentoId == orcamentoId)
                    .ToListAsync();

                return orcamentoItem;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao Obter Itens do Orçamento: {e.Message}");
            }
        }        
    }
}
