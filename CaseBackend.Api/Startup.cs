using CaseBackend.Api.HealthChecks;
using CaseBackend.Application.Query.Responses;
using CaseBackend.Infra.IoC;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using System;

namespace CaseBackend.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private const string HealthChecksUrl = "/health-check";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddHealthChecks()                
                    .AddTypeActivatedCheck<InvestmentsApiHealthCheck>(Guid.NewGuid().ToString(), "Tesouro Direto", "http://www.mocky.io/v2/5e3428203000006b00d9632a")
                    .AddTypeActivatedCheck<InvestmentsApiHealthCheck>(Guid.NewGuid().ToString(), "Fundos", "http://www.mocky.io/v2/5e342ab33000008c00d96342")
                    .AddTypeActivatedCheck<InvestmentsApiHealthCheck>(Guid.NewGuid().ToString(), "LCI", "http://www.mocky.io/v2/5e3429a33000008c00d96336");

            services
                .AddLogging()
                .AddMediatR(typeof(Response<>).Assembly)
                .AddMemoryCache()
                .AddCustomServices()
                .AddHealthChecksUI(setup => {
                    setup.AddHealthCheckEndpoint("Investments API Health Checks", HealthChecksUrl);                    
                    setup.SetEvaluationTimeInSeconds(TimeSpan.FromMinutes(1).Seconds);
                })
                .AddInMemoryStorage();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting()
               .UseHealthChecks("/health-check/status")
               .UseHealthChecksUI(setup =>
               {
                   setup.UIPath = "/health-check-ui";
               })
               .UseEndpoints(endpoints =>
               {
                   endpoints.MapControllers();

                   endpoints.MapHealthChecks(HealthChecksUrl, new HealthCheckOptions
                   {
                       Predicate = _ => true,
                       ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                   });

                   endpoints.MapHealthChecksUI();
               });
        }
    }
}
