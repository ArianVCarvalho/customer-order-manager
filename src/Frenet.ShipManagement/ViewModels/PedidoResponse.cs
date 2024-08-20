using Frenet.ShipManagement.DTOs;

namespace Frenet.ShipManagement.ViewModels
{
    public class PedidoResponse
    {
        public int ClienteId { get; set; }
        public PedidoDto Pedido { get; set; }
    }

}

