using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.Views.Dto;

namespace Frenet.ShipManagement.Repositories.Interface
{
    public interface IPedidoRepository
    {
        Task<Pedido> CreatePedido(PedidoDto pedido);
        Task<Pedido> GetPedidoById(int id);
        Task<List<Pedido>> GetPedidos();
        Task<List<Pedido>> GetPedidosByClienteId(int clienteId);
        Task <Pedido> UpdatePedido(int id, PedidoDto pedido);
    }
}