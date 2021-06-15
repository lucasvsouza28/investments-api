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

namespace CaseBackend.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services
                .AddLogging()
                .AddMediatR(typeof(Response<>).Assembly)
                .AddCustomServices(Configuration);
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
                   setup.ApiPath = "/health-check-ui-api";
               })
               .UseEndpoints(endpoints =>
               {
                   endpoints.MapControllers();

                   endpoints.MapHealthChecks("/health-check", new HealthCheckOptions
                   {
                       Predicate = _ => true,
                       ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                   });

                   endpoints.MapHealthChecksUI();
               });
        }
    }
}
