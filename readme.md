# Case Backend - Investments API

API para consolidar dados de investimentos de fontes distintas (ex: Tesouro direto, LCI, Fundos de investimento).

## Tecnologias utilizadas
- .NET Core 3.1

## Práticas aplicadas / Bibliotecas utilizadas
- SOLID
- CQRS / Mediatr
- Dependency Injection / .NET Core
- Unit Testing / NUnit, NSubstitute
- Redis Distributed Cache
- Health Checks (DNS, TCP, Redis)

## Como executar a aplicação
- Clonar esse repositório
- Abrir a solução com Visual Studio (.sln)
- Executar aplicação (F5)
- Acessar endpoint através da url: http://localhost:4525/investments

## Como executar os testes
- Com a solução (.sln) aberta no Visual Studio, acessar o menu Test -> Test Explorer
- Clicar no botão "Executar"

## Endpoints disponíveis
- ``/investments`` Retorna dados consolidados dos investimentos
- ``/health-check`` Retorna status detalhado de cada health check
- ``/health-check/status`` Retorna status geral dos health checks
- ``/health-check-ui`` Interface para visualizar status dos health checks

> Você pode acessar a aplicação publicada na Azure, clicando [aqui](http://investments-api.azurewebsites.net/health-check-ui)