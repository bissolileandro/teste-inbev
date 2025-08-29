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
    public class OrcamentoService : ServiceBase<Orcamento>, IOrcamentoService
    {
        private IOrcamentoRepository orcamentoRepository;
        public OrcamentoService(IOrcamentoRepository orcamentoRepository)
            :base(orcamentoRepository) 
        {
            this.orcamentoRepository = orcamentoRepository;
        }
        public async Task<Orcamento> ObterPorId(int id)
        {
            return await orcamentoRepository.ObterPorId(id);
        }        

        public override async Task<IEnumerable<Orcamento>> ListarTodosAsync()
        {
            return await orcamentoRepository.ListarTodosAsync();
        }

        public override async Task<Orcamento> DeleteAnsync(Orcamento obj)
        {
            return await orcamentoRepository.DeleteAnsync(obj);
        }

        
    }
}
