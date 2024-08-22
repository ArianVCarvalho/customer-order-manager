using Frenet.ShipManagement.DTOs;
using Frenet.ShipManagement.Services.Interface;
using Frenet.ShipManagement.Validators;
using Frenet.ShipManagement.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using static Frenet.ShipManagement.Models.ShippingResponseFrenet;

namespace Frenet.ShipManagement.Services
{
    public class ShippingService : IShippingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        private readonly string _accessToken;
        private readonly ILogger<ShippingService> _logger;

        public ShippingService(HttpClient httpClient, IConfiguration configuration, ILogger<ShippingService> logger)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["FreteApiConfig:FreteApiBaseUrl"] ?? throw new ArgumentNullException("FreteApiBaseUrl não configurada.");
            _accessToken = configuration["FreteApiConfig:AccessToken"] ?? throw new ArgumentNullException("AccessToken não configurado.");
            _logger = logger;
        }

        public async Task<Result<ShippingResponse>> CalcularFrete(SimulacaoDto cotacao)
        {
            if (cotacao == null)
            {
                _logger.LogWarning("Dados de cotação não podem ser nulos.");
                return Result<ShippingResponse>.Failure(400, "Dados de cotação não podem ser nulos.");
            }

            if (!CepValidator.IsValid(cotacao.Origem) || !CepValidator.IsValid(cotacao.Destino))
            {
                _logger.LogWarning("Os CEPs fornecidos são inválidos: Origem: {Origem}, Destino: {Destino}", cotacao.Origem, cotacao.Destino);
                return Result<ShippingResponse>.Failure(400, "Os CEPs fornecidos são inválidos.");
            }

            try
            {
                string apiUrl = $"{_apiBaseUrl}/shipping/quote";
                _logger.LogInformation("Enviando requisição para a API de frete: {ApiUrl}", apiUrl);

                var requestData = new
                {
                    SellerCEP = cotacao.Origem,
                    RecipientCEP = cotacao.Destino,
                    ShipmentInvoiceValue = 320.685,
                    ShippingServiceCode = (string)null,
                    RecipientCountry = "BR",
                    ShippingItemArray = new object[]
                    {
                        new
                        {
                            Height = 2,
                            Length = 33,
                            Quantity = 1,
                            Weight = 1.18,
                            Width = 47,
                            SKU = "IDW_54626",
                            Category = "Running"
                        },
                        new
                        {
                            Height = 5,
                            Length = 15,
                            Quantity = 1,
                            Weight = 0.5,
                            Width = 29
                        }
                    }
                };

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl)
                {
                    Content = JsonContent.Create(requestData)
                };

                requestMessage.Headers.Add("Accept", "application/json");
                requestMessage.Headers.Add("token", _accessToken);

                HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Resposta recebida com sucesso da API de frete.");

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var responseObject = await response.Content.ReadFromJsonAsync<ShippingResponseWrapper>();

                    var firstService = responseObject?.ShippingSevicesArray?[0];

                    if (firstService != null)
                    {
                        var shippingPrice = Convert.ToDecimal(firstService.ShippingPrice);
                        var deliveryTime = Convert.ToInt32(firstService.DeliveryTime);

                        var result = new ShippingResponse
                        {
                            ShippingPrice = shippingPrice,
                            OriginalDeliveryTime = deliveryTime
                        };

                        _logger.LogInformation("Frete calculado com sucesso: Preço: {ShippingPrice}, Tempo de Entrega: {DeliveryTime}", shippingPrice, deliveryTime);
                        return Result<ShippingResponse>.Success(result);
                    }

                    _logger.LogWarning("Nenhum serviço de frete encontrado na resposta.");
                    return Result<ShippingResponse>.Failure(500, "Nenhum serviço de frete encontrado.");
                }
                else
                {
                    _logger.LogWarning("Falha ao consultar a API de frete. Código de status: {StatusCode}", response.StatusCode);
                    return Result<ShippingResponse>.Failure((int)response.StatusCode, "Falha ao consultar a API de frete.");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Erro ao consultar frete: {Message}", ex.Message);
                return Result<ShippingResponse>.Failure(500, $"Erro ao consultar frete: {ex.Message}");
            }
        }
    }
}
