using Microsoft.Extensions.Options;
using Sample.Domain.Options;
using System;
using System.Data;
using System.Data.SqlClient;


namespace Sample.Infra.Persistence.Context
{
   public class DbContext : IDbContext
   {
        public IDbConnection _dbConnection;
        private AppConfigOptions _appConfigOptions;
        private bool _disposed = false;


        public DbContext(IOptions<AppConfigOptions> option)
        {
            _appConfigOptions = option.Value;
        }
        public IDbConnection GetConnection()
        {
            if (_dbConnection == null)
                _dbConnection = new SqlConnection(_appConfigOptions.ConnectionString);
            return _dbConnection;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                if (_dbConnection != null)
                {
                    _dbConnection.Dispose();
                    _dbConnection = null;
                }
            }
            _disposed = true;
        }
    }
}
