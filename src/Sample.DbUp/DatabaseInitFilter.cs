using DbUp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Sample.Domain.Options;
using System;

namespace Sample.DbUp
{
    public class DatabaseInitFilter : IStartupFilter
    {
        private AppConfigOptions _appConfigOptions;
        private DbLogger<DatabaseInitFilter> _logger;

        public DatabaseInitFilter(IOptions<AppConfigOptions> options, DbLogger<DatabaseInitFilter> dbLogger)
        {
            _appConfigOptions = options.Value;
            _logger = dbLogger;
        }
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            EnsureDatabase.For.PostgresqlDatabase(_appConfigOptions.ConnectionString);
            var builder = DeployChanges.To.PostgresqlDatabase(_appConfigOptions.ConnectionString)
                                         .WithScriptsEmbeddedInAssembly(typeof(DatabaseInitFilter).Assembly)
                                         .WithTransaction()
                                         .LogTo(_logger);
            var upgradeEngine = builder.Build();
            if (upgradeEngine.IsUpgradeRequired())
            {
                _logger.WriteInformation("Upgrade has been detected. Upgrading Database now .");
                var oper = upgradeEngine.PerformUpgrade();
                if (oper.Successful)
                    _logger.WriteInformation("Upgrade was successful");
                else
                    _logger.WriteError("Upgrade failed . Check logs for error");

            }
            return next;
        }
    }
}
