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

        public PedidoService(IPedidoRepository repository)
        {
            this.pedidoRepository = repository;
        }
        public async Task<List<Pedido>> GetPedidos()
        {
            return await pedidoRepository.GetPedidos();
        }
        public async Task<Pedido> CreatePedido(PedidoDto pedido)
        {
            return await pedidoRepository.CreatePedido(pedido);
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