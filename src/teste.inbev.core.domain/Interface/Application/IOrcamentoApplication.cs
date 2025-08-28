using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teste.inbev.core.domain.Entities;
using teste.inbev.core.domain.Models;

namespace teste.inbev.core.domain.Interface.Application
{
    public interface IOrcamentoApplication
    {
        Task<List<OrcamentoResponseModel>> ListarTodosAsync();
        Task<OrcamentoResponseModel> ObterPorId(int id);        
        Task<OrcamentoResponseModel> InserirAsync(OrcamentoRequestModel orcamentoModel);
        Task<OrcamentoResponseModel> AtualizarAsync(OrcamentoRequestModel orcamentoModel);
        Task<OrcamentoResponseModel> DeleteAnsync(int id);
    }
}
