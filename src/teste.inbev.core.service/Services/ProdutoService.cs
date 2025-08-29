using teste.inbev.core.domain.Entities;
using teste.inbev.core.domain.Interface.Repositories;
using teste.inbev.core.domain.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teste.inbev.core.service.Services
{
    public class ProdutoService : ServiceBase<Produto>, IProdutoService
    {
        public readonly IProdutoRepository produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository)
            : base(produtoRepository)
        {
            this.produtoRepository = produtoRepository;
        }        

        public async Task<Produto> ObterPorId(int id)
        {
            return await produtoRepository.ObterPorId(id);
        }
    }
}
