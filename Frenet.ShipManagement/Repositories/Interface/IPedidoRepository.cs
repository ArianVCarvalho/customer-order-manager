using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.Views.Dto;
using Frenet.ShipManagement.Views.Request;
using Frenet.ShipManagement.Views.Response;

namespace Frenet.ShipManagement.Repositories.Interface
{
    public interface IPedidoRepository
    {
        Task<Pedido> CreatePedido(Pedido pedido);
        Task<Pedido> GetPedidoById(int id);
        Task<List<PedidoResponse>> GetPedidos();
        Task<List<PedidoResponse>> GetPedidosByClienteId(int clienteId);
        Task<Pedido> UpdatePedido(int id, PedidoRequest pedido, decimal frete);
        Task<Pedido> UpdateStatus(int id, Status status);
    }
}