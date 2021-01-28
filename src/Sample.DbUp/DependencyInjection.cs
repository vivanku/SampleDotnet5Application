using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sample.DbUp
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDbUp(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IStartupFilter, DatabaseInitFilter>();
            services.AddTransient(typeof(DbLogger<>));
            return services;
        }
    }
}
