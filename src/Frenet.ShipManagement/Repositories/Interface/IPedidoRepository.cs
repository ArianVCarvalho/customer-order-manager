using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.ViewModels;

namespace Frenet.ShipManagement.Repositories.Interface
{
    public interface IPedidoRepository
    {
        Task<Pedido> CreatePedido(Pedido pedido);
        Task<Pedido> GetPedidoById(int id);
        Task<List<PedidoResponse>> GetPedidos(CancellationToken cancellation);
        Task<List<PedidoResponse>> GetPedidosByClienteId(int clienteId);
        Task Remove(Pedido pedido);
        Task<Pedido> UpdatePedido(int id, PedidoRequest pedido, decimal frete);
        Task<Pedido> UpdateStatus(int id, Status status);
    }
}