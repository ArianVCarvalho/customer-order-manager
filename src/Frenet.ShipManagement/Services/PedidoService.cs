using Frenet.ShipManagement.DTOs;
using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.Repositories.Interface;
using Frenet.ShipManagement.Services.Interface;
using Frenet.ShipManagement.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frenet.ShipManagement.Services
{
    /// <summary>
    /// Serviço responsável pela lógica de negócios relacionada aos pedidos.
    /// </summary>
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IShippingService _shippingService;
        private readonly ILogger<PedidoService> _logger;

        /// <summary>
        /// Inicializa uma nova instância do serviço de pedidos.
        /// </summary>
        /// <param name="repository">Repositório de pedidos utilizado pelo serviço.</param>
        /// <param name="shippingService">Serviço de cálculo de frete utilizado pelo serviço.</param>
        /// <param name="logger">Logger para registrar eventos e erros.</param>
        public PedidoService(IPedidoRepository repository, IShippingService shippingService, ILogger<PedidoService> logger)
        {
            _pedidoRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            _shippingService = shippingService ?? throw new ArgumentNullException(nameof(shippingService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtém uma lista dos 10 pedidos mais recentes.
        /// </summary>
        /// <returns>Uma lista de objetos <see cref="PedidoResponse"/> representando os pedidos mais recentes.</returns>
        public async Task<List<PedidoResponse>> GetPedidos()
        {
            _logger.LogInformation("Obtendo lista dos 10 pedidos mais recentes.");
            var pedidos = await _pedidoRepository.GetPedidos();
            _logger.LogInformation("Lista de pedidos obtida com sucesso. Total de pedidos: {Count}", pedidos.Count);
            return pedidos;
        }

        /// <summary>
        /// Cria um novo pedido com base nas informações fornecidas.
        /// </summary>
        /// <param name="pedido">Objeto <see cref="PedidoRequest"/> contendo as informações do novo pedido.</param>
        /// <returns>O objeto <see cref="Pedido"/> criado.</returns>
        public async Task<Pedido> CreatePedido(PedidoRequest pedido)
        {
            _logger.LogInformation("Criando novo pedido. ClienteId: {ClienteId}, Origem: {Origem}, Destino: {Destino}", pedido.ClienteId, pedido.Origem, pedido.Destino);

            var simulacao = new SimulacaoDto
            {
                Destino = pedido.Destino,
                Origem = pedido.Origem,
            };

            var frete = await _shippingService.CalcularFrete(simulacao);

            if (frete.IsSuccess)
            {
                var criarPedido = new Pedido
                {
                    ClienteId = pedido.ClienteId,
                    Destino = pedido.Destino,
                    Status = Status.Processamento,
                    Origem = pedido.Origem,
                    ValorFrete = frete.Value.ShippingPrice
                };

                var pedidoCriado = await _pedidoRepository.CreatePedido(criarPedido);
                _logger.LogInformation("Pedido criado com sucesso. PedidoId: {PedidoId}", pedidoCriado.Id);
                return pedidoCriado;
            }
            else
            {
                _logger.LogWarning("Falha ao calcular frete para o pedido. Mensagem: {Message}", frete.ErrorMessage);
                throw new Exception($"Falha ao calcular frete: {frete.ErrorMessage}");
            }
        }

        /// <summary>
        /// Obtém um pedido pelo identificador.
        /// </summary>
        /// <param name="id">Identificador do pedido.</param>
        /// <returns>O objeto <see cref="Pedido"/> correspondente ao identificador fornecido.</returns>
        public async Task<Pedido> GetPedidoById(int id)
        {
            _logger.LogInformation("Obtendo pedido pelo identificador: {Id}", id);
            var pedido = await _pedidoRepository.GetPedidoById(id);
            if (pedido != null)
            {
                _logger.LogInformation("Pedido encontrado. PedidoId: {PedidoId}", pedido.Id);
            }
            else
            {
                _logger.LogWarning("Pedido não encontrado. PedidoId: {PedidoId}", id);
            }
            return pedido;
        }

        /// <summary>
        /// Obtém uma lista de pedidos associados a um cliente específico.
        /// </summary>
        /// <param name="clienteId">Identificador do cliente.</param>
        /// <returns>Uma lista de objetos <see cref="Pedido"/> associados ao cliente especificado.</returns>
        public async Task<List<PedidoResponse>> GetPedidosByClienteId(int clienteId)
        {
            _logger.LogInformation("Obtendo pedidos para o clienteId: {ClienteId}", clienteId);
            var pedidos = await _pedidoRepository.GetPedidosByClienteId(clienteId);
            _logger.LogInformation("Pedidos obtidos para o clienteId: {ClienteId}. Total de pedidos: {Count}", clienteId, pedidos.Count);
            return pedidos;
        }

        /// <summary>
        /// Atualiza um pedido existente com as informações fornecidas.
        /// </summary>
        /// <param name="id">Identificador do pedido a ser atualizado.</param>
        /// <param name="pedido">Objeto <see cref="PedidoDto"/> contendo as novas informações do pedido.</param>
        /// <returns>O objeto <see cref="Pedido"/> atualizado.</returns>
        public async Task<Pedido> UpdatePedido(int id, PedidoRequest pedido)
        {
            _logger.LogInformation("Atualizando pedido. PedidoId: {PedidoId}", id);

            var simulacao = new SimulacaoDto
            {
                Destino = pedido.Destino,
                Origem = pedido.Origem,
            };

            var frete = await _shippingService.CalcularFrete(simulacao);

            if (frete.IsSuccess)
            {
                var pedidoAtualizado = await _pedidoRepository.UpdatePedido(id, pedido, frete.Value.ShippingPrice);
                _logger.LogInformation("Pedido atualizado com sucesso. PedidoId: {PedidoId}", id);
                return pedidoAtualizado;
            }
            else
            {
                _logger.LogWarning("Falha ao calcular frete para a atualização do pedido. Mensagem: {Message}", frete.ErrorMessage);
                throw new Exception($"Falha ao calcular frete: {frete.ErrorMessage}");
            }
        }

        public async Task<Pedido> UpdateStatus(int id, Status status)
        {
            _logger.LogInformation("Atualizando status do pedido. PedidoId: {PedidoId}, NovoStatus: {Status}", id, status);
            var pedidoAtualizado = await _pedidoRepository.UpdateStatus(id, status);
            _logger.LogInformation("Status do pedido atualizado com sucesso. PedidoId: {PedidoId}", id);
            return pedidoAtualizado;
        }

        public async Task Remove(Pedido pedido)
        {
            _logger.LogInformation("Removendo pedido. PedidoId: {PedidoId}", pedido.Id);
            await _pedidoRepository.Remove(pedido);
            _logger.LogInformation("Pedido removido com sucesso. PedidoId: {PedidoId}", pedido.Id);
        }
    }
}
