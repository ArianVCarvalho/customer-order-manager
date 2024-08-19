using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.Views.Dto;
using Frenet.ShipManagement.Views.Request;
using Frenet.ShipManagement.Views.Response;

namespace Frenet.ShipManagement.Services.Interface
{
    public interface IPedidoService
    {
        Task<List<PedidoResponse>> GetPedidos();
        Task<Pedido> CreatePedido(PedidoRequest pedido);
        Task<Pedido> GetPedidoById(int id);
        Task<List<PedidoResponse>> GetPedidosByClienteId(int clienteId);
        Task<Pedido> UpdatePedido(int id, PedidoRequest pedido);
    }
}