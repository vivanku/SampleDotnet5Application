using Dapper;
using Microsoft.Extensions.Logging;
using Sample.Application.Repository;
using Sample.Infra.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions;

namespace Sample.Infra.Persistence.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private IDbConnection _dbConnection;
        private ILogger<GenericRepository<TEntity>> _logger;

        public GenericRepository(IDbContext dbContext,ILogger<GenericRepository<TEntity>> logger)
        {
            _dbConnection = dbContext.GetConnection();
            _logger = logger;
        }
        public async Task Add(TEntity entity)
        {
            try
            {
                await _dbConnection.InsertAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured", ex);
                throw;
            }
        }
        public async Task Update(TEntity entity)
        {
            try
            {
                await _dbConnection.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured", ex);
                throw;
            }
        }
        public async Task Delete(TEntity entity)
        {
            try
            {
                await _dbConnection.DeleteAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured", ex);
                throw;
            }
        }

        public async Task<TEntity> Get(string Id)
        {
            try
            {
               return await _dbConnection.GetAsync<TEntity>(Id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured", ex);
                throw;
            }
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            try
            {
                return await _dbConnection.GetListAsync<TEntity>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured", ex);
                return new List<TEntity>();
            }
        }

        public async Task<IEnumerable<TEntity>> Query(string query, object parameters = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                return await _dbConnection.QueryAsync<TEntity>(query, parameters, commandType:commandType);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured",ex);
                return new List<TEntity>();
            }
        }

        public async Task<TEntity> QuerySingle(string query, object parameters = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                return await _dbConnection.QuerySingleAsync<TEntity>(query, parameters, commandType: commandType);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured", ex);
                throw;
            }
        }

        public async Task Execute(string query, object parameters = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                 await _dbConnection.ExecuteAsync(query, parameters, commandType: commandType);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured", ex);
                throw;
            }
        }
    }
}
