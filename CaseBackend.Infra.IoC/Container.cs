using System;
using System.Linq;
using System.Text.RegularExpressions;
using CaseBackend.Application.Domain.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                .AddCustomHealthChecksWithUI(configuration)
                .Configure<EndpointsSettings>(configuration.GetSection("EndpointsSettings"));

        private static IServiceCollection AddCache(this IServiceCollection services, IConfiguration configuration)
        {
            var redisCn = configuration.GetConnectionString("redis");

            if (string.IsNullOrEmpty(redisCn))
                return services.AddDistributedMemoryCache();


            return services.AddStackExchangeRedisCache(rOptions => rOptions.Configuration = redisCn);
        }

        private static IServiceCollection AddCustomHealthChecksWithUI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecksUI(setup =>
                {
                    var urls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS").Split(';');
                    var uris = urls.Select(url => Regex.Replace(url, @"^(?<scheme>https?):\/\/((\+)|(\*)|(0.0.0.0))(?=[\:\/]|$)", "${scheme}://localhost"))
                                    .Select(uri => new Uri(uri, UriKind.Absolute)).ToArray();

                    var httpEndpoint = uris.FirstOrDefault(uri => uri.Scheme == "http");
                    var httpsEndpoint = uris.FirstOrDefault(uri => uri.Scheme == "https");
                    
                    if (httpEndpoint != null) // Create an HTTP healthcheck endpoint
                    {
                        setup.AddHealthCheckEndpoint("Investments API", new UriBuilder(httpEndpoint.Scheme, httpEndpoint.Host, httpEndpoint.Port, "/health-check").ToString());
                    }
                    
                    if (httpsEndpoint != null) // Create an HTTPS healthcheck endpoint
                    {
                        setup.AddHealthCheckEndpoint("Investments API", new UriBuilder(httpsEndpoint.Scheme, httpsEndpoint.Host, httpsEndpoint.Port, "/health-check").ToString());
                    }
                    
                    setup.SetEvaluationTimeInSeconds(TimeSpan.FromMinutes(1).Seconds);
                })
                .AddInMemoryStorage();
            
            services.AddHealthChecks()
                    .AddDnsResolveHealthCheck(setup => setup.ResolveHost("www.mocky.io"), name: "DNS Resolve Health Check 'www.mocky.io'")
                    .AddTcpHealthCheck(setup => setup.AddHost("www.mocky.io", 80), name: "TCP Health Check", timeout: TimeSpan.FromMinutes(2))
                    .AddRedis(configuration.GetConnectionString("redis"), "Redis Health Check", timeout: TimeSpan.FromMinutes(2));

            return services;
        }
    }
}
