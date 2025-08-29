using teste.inbev.core.domain.Interface.Repositories;
using teste.inbev.core.domain.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teste.inbev.core.service.Services
{
    public class ServiceBase<TEntity> : IServiceBase<TEntity> where TEntity : class
    {
        private IRepositoryBase<TEntity> repository;

        public ServiceBase(IRepositoryBase<TEntity> repository)
        {
            this.repository = repository;
        }
        public void Add(TEntity obj)
        {
            repository.Add(obj);
        }

        public async Task<TEntity> InserirAsync(TEntity obj)
        {
            return await repository.InserirAsync(obj);
        }

        public virtual async Task<TEntity> DeleteAnsync(TEntity obj)
        {
            return await repository.DeleteAnsync(obj);
        }

        public IEnumerable<TEntity> ListarTodos()
        {
            return repository.ListarTodos();
        }

        public virtual async Task<IEnumerable<TEntity>> ListarTodosAsync()
        {
            return await repository.ListarTodosAsync();
        }

        public async Task<TEntity> AtualizarAsync(TEntity obj)
        {
            return await repository.AtualizarAsync(obj);
        }
    }
}
