using Microsoft.EntityFrameworkCore;
using teste.inbev.core.data.Context;
using teste.inbev.core.domain.Entities;
using teste.inbev.core.domain.Interface.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teste.inbev.core.data.Repositories
{
    public class ProdutoRepository : RepositoryBase<Produto>, IProdutoRepository
    {
        public ProdutoRepository(InbevCoreContext context)
            : base(context) 
        {
            
        }

        public async Task<Produto> ObterPorId(int id)
        {
            try
            {
                var produto = await db.Set<Produto>().FirstOrDefaultAsync(i => i.Id == id);

                return produto;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao Obter Produto: {e.Message}");
            }
        }        
    }
}
