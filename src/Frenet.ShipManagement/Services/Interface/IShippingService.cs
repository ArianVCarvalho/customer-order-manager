using Frenet.ShipManagement.DTOs;
using Frenet.ShipManagement.ViewModels;

namespace Frenet.ShipManagement.Services.Interface
{
    /// <summary>
    /// Interface que define o contrato para os serviços relacionados ao cálculo de frete.
    /// </summary>
    public interface IShippingService
    {
        /// <summary>
        /// Calcula o valor do frete com base nos dados de simulação fornecidos.
        /// </summary>
        /// <param name="cotacao">Objeto <see cref="SimulacaoDto"/> contendo os CEPs de origem e destino para o cálculo do frete.</param>
        /// <returns>
        /// Um <see cref="Task{TResult}"/> contendo um <see cref="Result{ShippingResponse}"/>. O resultado encapsula
        /// o cálculo do frete, incluindo sucesso ou falha, e as informações de resposta do frete.
        /// </returns>
        Task<Result<ShippingResponse>> CalcularFrete(SimulacaoDto cotacao);
    }
}
