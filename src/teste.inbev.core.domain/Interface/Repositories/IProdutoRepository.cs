using teste.inbev.core.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teste.inbev.core.domain.Interface.Repositories
{
    public interface IProdutoRepository : IRepositoryBase<Produto>
    {
        Task<Produto> ObterPorId(int id);        
    }
}
