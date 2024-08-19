using Frenet.ShipManagement.Views.Dto;

namespace Frenet.ShipManagement.Views.Response
{
    public class PedidoResponse
    {
        public int ClienteId { get; set; }
        public PedidoDto Pedido { get; set; }
    }

}

