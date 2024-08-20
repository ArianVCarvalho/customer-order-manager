using Frenet.ShipManagement.DTOs;
using Frenet.ShipManagement.Services.Interface;
using Frenet.ShipManagement.Validators;
using Frenet.ShipManagement.ViewModels;
using static Frenet.ShipManagement.Models.ShippingResponseFrenet;

namespace Frenet.ShipManagement.Services
{
    public class ShippingService : IShippingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        private readonly string _accessToken;

        public ShippingService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiBaseUrl = configuration["FreteApiConfig:FreteApiBaseUrl"] ?? throw new ArgumentNullException("FreteApiBaseUrl não configurada.");
            _accessToken = configuration["FreteApiConfig:AccessToken"] ?? throw new ArgumentNullException("AccessToken não configurado.");
        }

        public async Task<Result<ShippingResponse>> CalcularFrete(SimulacaoDto cotacao)
        {
            if (cotacao == null)
            {
                return Result<ShippingResponse>.Failure(400, "Dados de cotação não podem ser nulos.");
            }

            if (!CepValidator.IsValid(cotacao.Origem) || !CepValidator.IsValid(cotacao.Destino))
            {
                return Result<ShippingResponse>.Failure(400, "Os CEPs fornecidos são inválidos.");
            }

            try
            {
                // Define o endpoint completo da API de frete
                string apiUrl = $"{_apiBaseUrl}/shipping/quote";

                // Cria o objeto que será enviado no corpo da requisição
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
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseBody);

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

                        return Result<ShippingResponse>.Success(result);
                    }

                    return Result<ShippingResponse>.Failure(500, "Nenhum serviço de frete encontrado.");
                }
                else
                {
                    return Result<ShippingResponse>.Failure((int)response.StatusCode, "Falha ao consultar a API de frete.");
                }
            }
            catch (HttpRequestException ex)
            {
                return Result<ShippingResponse>.Failure(500, $"Erro ao consultar frete: {ex.Message}");
            }
        }
    }
}
