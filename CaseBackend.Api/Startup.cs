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

        private const string HealthChecksUrl = "http:////local/health-check";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services
                .AddLogging()
                .AddMediatR(typeof(Response<>).Assembly)
                .AddCustomServices(Configuration)
                .AddHealthChecksUI(setup =>
                {
                    //setup.AddHealthCheckEndpoint("Investments API Health Checks", "healthz");
                    setup.SetEvaluationTimeInSeconds(TimeSpan.FromMinutes(1).Seconds);
                })
                .AddInMemoryStorage();

            services.AddHealthChecks()
                    .AddDnsResolveHealthCheck(setup => setup.ResolveHost("www.mocky.io"), name: "Valida DNS 'www.mocky.io'")
                    .AddTcpHealthCheck(setup => setup.AddHost("www.mocky.io", 80), name: "Valida acesso ao host 'www.mocky.io'", timeout: TimeSpan.FromMinutes(5))
                    ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting()
               //.UseHealthChecks("/healthz")
               .UseHealthChecksUI(setup =>
               {
                   setup.UIPath = "/health-check-ui";
                   setup.ApiPath = "/health-check-ui-api";
               })
               .UseEndpoints(endpoints =>
               {
                   endpoints.MapControllers();

                   endpoints.MapHealthChecks("healthz", new HealthCheckOptions
                   {
                       Predicate = _ => true,
                       ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                   });

                   endpoints.MapHealthChecksUI();
               });
        }
    }
}
