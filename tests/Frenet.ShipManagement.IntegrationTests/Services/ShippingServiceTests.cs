﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Xunit;
using Frenet.ShipManagement.DTOs;
using Frenet.ShipManagement.Services;
using Frenet.ShipManagement.ViewModels;

namespace Frenet.ShipManagement.IntegrationTests.Services
{
    public class ShippingServiceTests : IDisposable
    {
        private HttpClient _httpClient;
        private ShippingService _shippingService;
        private readonly ILogger<ShippingService> _logger;

        public ShippingServiceTests()
        {
            _logger = new LoggerFactory().CreateLogger<ShippingService>();

            _httpClient = new HttpClient();

            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<ShippingService>()  // Carrega segredos do User Secrets
                .AddEnvironmentVariables()           // Carrega variáveis de ambiente
                .Build();

            var apiBaseUrl = configuration["FreteApiConfig:FreteApiBaseUrl"]
                ?? configuration["FRETE_API_BASE_URL"]
                ?? throw new InvalidOperationException("apiBaseUrl não configurado.");

            var accessToken = configuration["FreteApiConfig:AccessToken"]
                ?? configuration["ACCESSTOKEN"]
                ?? throw new InvalidOperationException("AccessToken não configurado.");

            _shippingService = new ShippingService(_httpClient, configuration, _logger);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        [Fact]
        public void Should_Load_Configuration()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<ShippingService>()  
                .AddEnvironmentVariables()
                .Build();

            var apiBaseUrl = configuration["FreteApiConfig:FreteApiBaseUrl"];
            var accessToken = configuration["FreteApiConfig:AccessToken"];

            Assert.NotNull(apiBaseUrl);
            Assert.NotNull(accessToken);
        }

        [Fact]
        public async Task Should_Calculate_Shipping()
        {
            var simulacaoDto = new SimulacaoDto
            {
                Origem = "19906150",
                Destino = "59152180"
            };

            var result = await _shippingService.CalcularFrete(simulacaoDto);

            Assert.True(result.IsSuccess);

            Assert.NotNull(result.Value);
            Assert.True(result.Value.ShippingPrice > 0);
            Assert.True(result.Value.OriginalDeliveryTime > 0);
        }
    }
}
