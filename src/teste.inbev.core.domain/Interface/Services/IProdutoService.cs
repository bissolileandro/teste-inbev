using teste.inbev.core.domain.Entities;
using teste.inbev.core.domain.Interface.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teste.inbev.core.domain.Interface.Services
{
    public interface IProdutoService : IServiceBase<Produto>
    {
        Task<Produto> ObterPorId(int id);        
    }
}
