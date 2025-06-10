using calculadora_sqia.API.Middleware;
using calculadora_sqia.Application.DTOs;
using calculadora_sqia.Core;
using System.Net;
using System.Text.Json;

namespace calculadora_sqia.API.Configuration
{
    public static class MiddlewareConfig
    {
        public static IApplicationBuilder AddCustomMiddlewares(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<GlobalErrorHandlingMiddleware>();

            return applicationBuilder;
        }
    }
}
