using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WireMock.Server;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;
using Frenet.ShipManagement.DTOs;
using Frenet.ShipManagement.Services;
using Frenet.ShipManagement.ViewModels;

namespace Frenet.ShipManagement.IntegrationTests.Services
{
    public class ShippingServiceTests : IAsyncLifetime
    {
        private WireMockServer _wireMockServer;
        private HttpClient _httpClient;
        private ShippingService _shippingService;
        private string _apiBaseUrl;
        private readonly ILogger<ShippingService> _logger;

        public ShippingServiceTests()
        {
            // Criação do mock do logger
            _logger = new LoggerFactory().CreateLogger<ShippingService>();
        }

        public async Task InitializeAsync()
        {
            // Inicializa o WireMockServer
            _wireMockServer = WireMockServer.Start();
            _apiBaseUrl = $"http://localhost:{_wireMockServer.Ports[0]}";
            _httpClient = new HttpClient();

            // Configura o ShippingService com o endpoint da API mock
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("FreteApiConfig:FreteApiBaseUrl", _apiBaseUrl),
                    new KeyValuePair<string, string>("FreteApiConfig:AccessToken", "dummy-token")
                })
                .Build();

            _shippingService = new ShippingService(_httpClient, configuration, _logger);

            ConfigureWireMockServer();
        }

        private void ConfigureWireMockServer()
        {
            _wireMockServer
                .Given(Request.Create().WithPath("/shipping/quote").UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBody(@"{
                        ""ShippingSevicesArray"": [
                            {
                                ""ShippingPrice"": ""320.68"",
                                ""DeliveryTime"": ""5""
                            }
                        ]
                    }")
                    .WithHeader("Content-Type", "application/json"));
        }

        public async Task DisposeAsync()
        {
            _wireMockServer.Stop();
            _httpClient.Dispose();
        }

        [Fact]
        public async Task Should_Calculate_Shipping()
        {
            var simulacaoDto = new SimulacaoDto
            {
                Origem = "12345678",
                Destino = "87654321"
            };

            var result = await _shippingService.CalcularFrete(simulacaoDto);

            Assert.True(result.IsSuccess);
            Assert.Equal(320.68m, result.Value.ShippingPrice);
            Assert.Equal(5, result.Value.OriginalDeliveryTime);
        }
    }
}
