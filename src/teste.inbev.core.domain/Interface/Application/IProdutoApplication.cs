using teste.inbev.core.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teste.inbev.core.domain.Models;

namespace teste.inbev.core.domain.Interface.Application
{
    public interface IProdutoApplication
    {
        Task<List<ProdutoModel>> ListarTodosAsync();
        Task<ProdutoModel> ObterPorId(int id);        
        Task<ProdutoModel> InserirAsync(ProdutoModel produtoModel);
        Task<ProdutoModel> AtualizarAsync(ProdutoModel produtoModel);
        Task<ProdutoModel> DeleteAnsync(int id);
    }
}
