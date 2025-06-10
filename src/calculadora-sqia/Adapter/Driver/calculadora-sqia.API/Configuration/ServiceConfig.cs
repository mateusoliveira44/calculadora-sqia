using calculadora_sqia.Application.Ports.Services;
using calculadora_sqia.Application.Ports.Services.Interfaces;

namespace calculadora_sqia.API.Configuration
{
    public static class ServiceConfig
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICarteiraService, CarteiraService>();
            return services;
        }
    }
}
