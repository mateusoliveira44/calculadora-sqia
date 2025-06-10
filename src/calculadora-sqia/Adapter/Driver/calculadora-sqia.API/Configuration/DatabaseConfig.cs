using calculadora_sqia.Application.Ports.Repositories.Interfaces;
using calculadora_sqia.Application.Ports.Services;
using calculadora_sqia.Application.Ports.Services.Interfaces;
using calculadora_sqia.SQLServer;

namespace calculadora_sqia.API.Configuration
{
    public static class DatabaseConfig
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Connection string 'DefaultConnection' não informado.", nameof(configuration));
            }

            services.AddSingleton<IDbConnectionFactory>(sp => new SqlConnectionFactory(connectionString));
            return services;
        }
        
    }
}
