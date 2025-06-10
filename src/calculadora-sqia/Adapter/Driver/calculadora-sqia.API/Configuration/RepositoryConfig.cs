using calculadora_sqia.Application.Ports.Repositories;
using calculadora_sqia.Application.Ports.Services;
using calculadora_sqia.Application.Ports.Services.Interfaces;
using calculadora_sqia.SQLServer.Repositories;

namespace calculadora_sqia.API.Configuration
{
    public static class RepositoryConfig
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICotacaoRepository, CotacaoRepository>();
            return services;
        }
    }
}
