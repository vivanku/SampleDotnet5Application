using DbUp.Engine.Output;
using Microsoft.Extensions.Logging;
using System;

namespace Sample.DbUp
{
    public class DbLogger<T> : IUpgradeLog where T : class
    {
        private ILogger<T> _logger;

        public DbLogger(ILogger<T> logger)
        {
            _logger = logger;
        }
        public void WriteError(string format, params object[] args)
        {
            _logger.LogError(format, args);
        }

        public void WriteInformation(string format, params object[] args)
        {
            _logger.LogInformation(format, args);
        }

        public void WriteWarning(string format, params object[] args)
        {
            _logger.LogWarning(format, args);
        }
    }
}
