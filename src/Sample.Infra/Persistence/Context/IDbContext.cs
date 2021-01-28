using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Infra.Persistence.Context
{
    public interface IDbContext : IDisposable
    {
        IDbConnection GetConnection();
    }
}
