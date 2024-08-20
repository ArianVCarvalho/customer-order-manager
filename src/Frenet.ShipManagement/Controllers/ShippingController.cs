using Frenet.ShipManagement.DTOs;
using Frenet.ShipManagement.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Frenet.ShipManagement.Controllers
{
    /// <summary>
    /// Controlador responsável por operações relacionadas ao cálculo de frete.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ShippingController : ControllerBase
    {
        private readonly IShippingService _shippingService;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="ShippingController"/>.
        /// </summary>
        /// <param name="shippingService">Serviço responsável pelo cálculo de frete.</param>
        public ShippingController(IShippingService shippingService)
        {
            _shippingService = shippingService;
        }

        /// <summary>
        /// Calcula o frete com base nos CEPs de origem e destino.
        /// </summary>
        /// <param name="cotacao">O DTO contendo os CEPs de origem e destino para o cálculo de frete.</param>
        /// <returns>O valor do frete calculado.</returns>
        /// <response code="200">Retorna o valor do frete calculado.</response>
        /// <response code="400">Se os dados da cotação não forem válidos.</response>
        /// <response code="500">Se ocorrer um erro ao consultar o serviço de frete.</response>
        [HttpPost("calcular")]
        public async Task<IActionResult> CalcularFrete([FromBody] SimulacaoDto cotacao)
        {
            try
            {
                var resultado = await _shippingService.CalcularFrete(cotacao);

                if (resultado.IsSuccess)
                {
                    return Ok(resultado.Value);
                }
                else
                {
                    return StatusCode(resultado.StatusCode, resultado.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao processar a solicitação: {ex.Message}");
            }
        }
    }
}
