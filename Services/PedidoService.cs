using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.Repositories.Interface;
using Frenet.ShipManagement.Services.Interface;
using Frenet.ShipManagement.Views.Dto;
using NuGet.Protocol.Core.Types;

namespace Frenet.ShipManagement.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository pedidoRepository;
        private readonly IShippingService _shippingService;

        public PedidoService(IPedidoRepository repository, IShippingService shippingService)
        {
            this.pedidoRepository = repository;
            _shippingService = shippingService;
        }
        public async Task<List<Pedido>> GetPedidos()
        {
            return await pedidoRepository.GetPedidos();
        }
        public async Task<Pedido> CreatePedido(PedidoDto pedido)
        {
            var simulacao = new SimulacaoDto
            {
                Destino = pedido.Destino,
                Origem = pedido.Origem,
            };

            var frete = _shippingService.CalcularFrete(simulacao);

            var criarPedido = new Pedido
            {
                ClienteId = pedido.ClienteId,
                Destino = pedido.Destino,
                Status = pedido.Status,
                Origem = pedido.Origem,
                ValorFrete = frete.Result.Value.ShippingPrice
            };

            return await pedidoRepository.CreatePedido(criarPedido);
        }

        public async Task<Pedido> GetPedidoById(int id)
        {
            return await pedidoRepository.GetPedidoById(id);
        }

        public async Task<List<Pedido>> GetPedidosByClienteId(int clienteId)
        {
            return await pedidoRepository.GetPedidosByClienteId(clienteId);
        }

        public async Task<Pedido> UpdatePedido(int id, PedidoDto pedido)
        {
            return await pedidoRepository.UpdatePedido(id, pedido);
        }
    }
}