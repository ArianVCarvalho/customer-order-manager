using Frenet.ShipManagement.Models;

namespace Frenet.ShipManagement.Views.Request
{
    public class PedidoRequest
    {
        /// <summary>
        /// Identificador do cliente que realizou o pedido.
        /// Este é o ID único do cliente no sistema, utilizado para associar o pedido ao cliente que o fez.
        /// </summary>
        public int ClienteId { get; set; }

        /// <summary>
        /// CEP de origem do pedido.
        /// Representa o código postal do local de onde o pedido está sendo enviado.
        /// </summary>
        public string Origem { get; set; }

        /// <summary>
        /// CEP de destino do pedido.
        /// Representa o código postal do local para onde o pedido deve ser entregue.
        /// </summary>
        public string Destino { get; set; }

        /// <summary>
        /// Status atual do pedido.
        /// Indica o estado atual do pedido, como "Em Processamento", "Enviado", "Entregue", etc.
        /// </summary>
        public Status Status { get; set; }
    }
}
