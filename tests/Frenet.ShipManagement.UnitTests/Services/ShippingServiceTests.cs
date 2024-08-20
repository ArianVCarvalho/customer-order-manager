using FluentAssertions;
using Frenet.ShipManagement.DTOs;
using Frenet.ShipManagement.Services;
using Frenet.ShipManagement.ViewModels;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;
using static Frenet.ShipManagement.Models.ShippingResponseFrenet;

namespace Frenet.ShipManagement.UnitTests.Services
{
    public class ShippingServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly ShippingService _shippingService;

        public ShippingServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "FreteApiConfig:FreteApiBaseUrl", "https://api.frete.com" },
                    { "FreteApiConfig:AccessToken", "fake_token" }
                })
                .Build();

            _shippingService = new ShippingService(_httpClient, configuration);
        }

        [Fact]
        public async Task CalcularFrete_ShouldReturnSuccess_WhenApiReturnsValidResponse()
        {
            var simulacaoDto = new SimulacaoDto
            {
                Origem = "11111111",
                Destino = "22222222"
            };

            var shippingResponseWrapper = new ShippingResponseWrapper
            {
                ShippingSevicesArray = new[]
                {
                    new ShippingServiceResponse
                    {
                        ShippingPrice = "100.0",
                        DeliveryTime = "5"
                    }
                }
            };

            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(shippingResponseWrapper)
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            var result = await _shippingService.CalcularFrete(simulacaoDto);
            
            result.Should().BeEquivalentTo(Result<ShippingResponse>.Success(new ShippingResponse
            {
                ShippingPrice = 1000.0m,
                OriginalDeliveryTime = 5
            }));
        }

        [Fact]
        public async Task CalcularFrete_ShouldReturnFailure_WhenApiReturnsError()
        {
            var simulacaoDto = new SimulacaoDto
            {
                Origem = "11111111",
                Destino = "22222222"
            };

            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = JsonContent.Create(new { message = "Bad Request" })
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            var result = await _shippingService.CalcularFrete(simulacaoDto);

            result.Should().BeEquivalentTo(Result<ShippingResponse>.Failure(400, "Falha ao consultar a API de frete."));
        }

        [Fact]
        public async Task CalcularFrete_ShouldReturnFailure_WhenResponseIsInvalid()
        {
            var simulacaoDto = new SimulacaoDto
            {
                Origem = "11111111",
                Destino = "22222222"
            };

            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(new { }) // Response without ShippingSevicesArray
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            var result = await _shippingService.CalcularFrete(simulacaoDto);

            result.Should().BeEquivalentTo(Result<ShippingResponse>.Failure(500, "Nenhum serviço de frete encontrado."));
        }

        [Fact]
        public async Task CalcularFrete_ShouldReturnFailure_WhenRequestFails()
        {
            var simulacaoDto = new SimulacaoDto
            {
                Origem = "11111111",
                Destino = "22222222"
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Request failed"));

            var result = await _shippingService.CalcularFrete(simulacaoDto);

            result.Should().BeEquivalentTo(Result<ShippingResponse>.Failure(500, "Erro ao consultar frete: Request failed"));
        }

        [Fact]
        public async Task CalcularFrete_ShouldReturnFailure_WhenInvalidCep()
        {
            // Arrange
            var simulacaoDto = new SimulacaoDto
            {
                Origem = "12345", 
                Destino = "54321"
            };

            var result = await _shippingService.CalcularFrete(simulacaoDto);

            result.Should().BeEquivalentTo(Result<ShippingResponse>.Failure(400, "Os CEPs fornecidos são inválidos."));
        }
    }
}
