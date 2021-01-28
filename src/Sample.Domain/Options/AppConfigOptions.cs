using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.Options
{
    public class AppConfigOptions
    {
        public string JwtSecret { get; set; }
        public string ConnectionString { get; set; }
    }
}
