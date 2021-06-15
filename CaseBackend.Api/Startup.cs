using CaseBackend.Application.Query.Responses;
using CaseBackend.Infra.IoC;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                    .AddTcpHealthCheck(setup => setup.AddHost("www.mocky.io", 80), name: "Valida acesso ao host 'www.mocky.io'");

            services
                .AddLogging()
                .AddMediatR(typeof(Response<>).Assembly)
                .AddCustomServices(Configuration)
                .AddHealthChecksUI(setup =>
                {
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
