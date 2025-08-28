using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using teste.inbev.core.data.Context;
using teste.inbev.core.domain.Entities;
using teste.inbev.core.domain.Interface.Repositories;

namespace teste.inbev.core.data.Repositories
{
    public class OrcamentoRepository : RepositoryBase<Orcamento>, IOrcamentoRepository
    {
        public OrcamentoRepository(InbevCoreContext context)
            : base(context)
        {

        }
        public async Task<Orcamento> ObterPorId(int id)
        {
            try
            {
                var orcamento = await db.Set<Orcamento>()
                    .Include(c => c.OrcamentoItems)
                    .ThenInclude(i => i.Produto)         
                    .FirstOrDefaultAsync(i => i.Id == id);

                return orcamento;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao Obter Orçamento: {e.Message}");
            }
        }
        public override async Task<IEnumerable<Orcamento>> ListarTodosAsync()
        {
            try
            {
                var orcamento = await db.Set<Orcamento>()
                    .Include(c => c.OrcamentoItems)
                    .ThenInclude(i => i.Produto).ToListAsync();

                return orcamento;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao Obter Orçamentos: {e.Message}");
            }
        }

        public override async Task<Orcamento> DeleteAnsync(Orcamento obj)
        {
            try
            {
                var orcamentoExcluir = await db.Set<Orcamento>()
                    .Include(c => c.OrcamentoItems)                    
                    .FirstOrDefaultAsync(i => i.Id == obj.Id);
                return await base.DeleteAnsync(orcamentoExcluir);
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao remver os dados: {e.Message}");
            }
        }
    }
}
