using Frenet.ShipManagement.Models;
using System;

namespace Frenet.ShipManagement.Views.Dto
{
    /// <summary>
    /// Representa os dados transferidos de um pedido no sistema de gestão de envios.
    /// </summary>
    public class PedidoDto
    {
        /// <summary>
        /// Identificador do cliente que realizou o pedido.
        /// </summary>
        public int ClienteId { get; set; }

        /// <summary>
        /// Cep Local de origem do pedido.
        /// </summary>
        public string Origem { get; set; }

        /// <summary>
        /// Cep de destino do pedido.
        /// </summary>
        public string Destino { get; set; }

        /// <summary>
        /// Status atual do pedido.
        /// </summary>
        public Status Status { get; set; }
    }
}