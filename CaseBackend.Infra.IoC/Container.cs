using CaseBackend.Application.Domain.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CaseBackend.Infra.IoC
{
    public static class Container
    {
        /// <summary>
        /// Configura injeção de dependência
        /// </summary>
        /// <param name="services">Coleção de serviços</param>
        /// <returns>Retorna <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection" />, possibilitando chamadas encadeadas.</returns>
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration) =>
              services
                .AddHttpClient()
                .AddCache(configuration)
                .Configure<EndpointsSettings>(configuration.GetSection("EndpointsSettings"));

        private static IServiceCollection AddCache(this IServiceCollection services, IConfiguration configuration)
        {

            var redisCn = configuration.GetConnectionString("redis");

            if (string.IsNullOrEmpty(redisCn))
                return services.AddDistributedMemoryCache();


            return services.AddStackExchangeRedisCache(rOptions => rOptions.Configuration = redisCn);



        }

    }
}
