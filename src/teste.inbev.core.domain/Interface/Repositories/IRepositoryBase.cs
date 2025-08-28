using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teste.inbev.core.domain.Interface.Repositories
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        void Add(TEntity obj);
        Task<TEntity> InserirAsync(TEntity obj);
        Task<TEntity> DeleteAnsync(TEntity obj);
        Task<TEntity> AtualizarAsync(TEntity obj);
        IEnumerable<TEntity> ListarTodos();
        Task<IEnumerable<TEntity>> ListarTodosAsync();
    }
}
