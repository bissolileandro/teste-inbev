using Microsoft.EntityFrameworkCore;
using teste.inbev.core.data.Context;
using teste.inbev.core.domain.Interface.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teste.inbev.core.data.Repositories
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        protected InbevCoreContext db;
        public RepositoryBase(InbevCoreContext db)
        {
            this.db = db;            
        }
        public void Add(TEntity obj)
        {
            try
            {
                db.Set<TEntity>().Add(obj);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao persistir os dados: {e.Message}");
            }
        }
        public async Task<TEntity> InserirAsync(TEntity obj)
        {
            try
            {
                await db.Set<TEntity>().AddAsync(obj);
                await db.SaveChangesAsync();
                return obj;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao persistir os dados: {e.Message}");
            }
        }
        public virtual async Task<TEntity> DeleteAnsync(TEntity obj)
        {
            try
            {
                db.Set<TEntity>().Remove(obj);
                await db.SaveChangesAsync();
                return obj;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao remver os dados: {e.Message}");
            }
        }
        public IEnumerable<TEntity> ListarTodos()
        {
            try
            {
                return db.Set<TEntity>().ToList();
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao consultar os dados: {e.Message}");
            }
        }
        public virtual async Task<IEnumerable<TEntity>> ListarTodosAsync()
        {
            try
            {
                return await db.Set<TEntity>().ToListAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao consultar os dados: {e.Message}");
            }
        }
        public async Task<TEntity> AtualizarAsync(TEntity obj)
        {
            try
            {
                db.Set<TEntity>().Update(obj);
                await db.SaveChangesAsync();
                return obj;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao atualizar os dados: {e.Message}");
            }
        }
    }
}
