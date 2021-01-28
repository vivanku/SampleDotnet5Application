using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Application.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> Get(string Id);
        Task<IEnumerable<TEntity>> GetAll();
        Task Add(TEntity entity);
        Task Delete(TEntity entity);
        Task Update(TEntity entity);
        Task<IEnumerable<TEntity>> Query(string query, object parameters = null , CommandType commandType = CommandType.Text);
        Task<TEntity> QuerySingle(string query, object parameters = null , CommandType commandType = CommandType.Text);
        Task Execute(string query, object parameters = null, CommandType commandType = CommandType.Text);

    }
}
