using Microsoft.Extensions.DependencyInjection;
using RestSharp;

namespace CaseBackend.Infra.IoC
{
    public static class Container
    {
        /// <summary>
        /// Configura injeção de dependência
        /// </summary>
        /// <param name="services">Coleção de serviços</param>
        /// <returns>Retorna <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection" />, possibilitando chamadas encadeadas.</returns>
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddSingleton<IRestClient, RestClient>();

            return services;
        }
    }
}
