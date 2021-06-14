using Microsoft.Extensions.Diagnostics.HealthChecks;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CaseBackend.Api.HealthChecks
{
    public class InvestmentsApiHealthCheck : IHealthCheck
    {
        public InvestmentsApiHealthCheck(string name, string url)
        {
            _name = name;
            _url = url;
            _restClient = new RestClient();
        }

        #region Fields

        private readonly string _name;
        private readonly string _url;
        private readonly IRestClient _restClient; 

        #endregion

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            context.Registration.Name = $"Verifica se o endpoint está acessível ({this._name})";

            var statusCode = await this.GetResponseStatusCode(out string errorMessage);

            if (statusCode != HttpStatusCode.OK)
            {
                string healthCheckMessage = !string.IsNullOrEmpty(errorMessage) ? errorMessage : $"Não foi possível consultar investimentos do endpoint '{this._url}'";
                var result = HealthCheckResult.Unhealthy(description: healthCheckMessage);

                return result;
            }

            return HealthCheckResult.Healthy(description: "O endpoint está acessível");
        }

        private Task<HttpStatusCode> GetResponseStatusCode(out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                IRestResponse response = _restClient.Get(new RestRequest(this._url));

                return Task.FromResult(response.StatusCode);
            }
            catch (Exception ex)
            {
                errorMessage = $"Ocorreu um erro ao requisitar o endpoint {this._url}\n: {ex}";
                return Task.FromResult(HttpStatusCode.InternalServerError);
            }
        }
    }
}
