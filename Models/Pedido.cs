using System;

namespace Frenet.ShipManagement.Models
{
    /// <summary>
    /// Representa um pedido no sistema de gestão de envios.
    /// </summary>
    public class Pedido
    {
        /// <summary>
        /// Identificador único do pedido.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Identificador do cliente que realizou o pedido.
        /// </summary>
        public int ClienteId { get; init; }

        /// <summary>
        /// O cliente associado a este pedido.
        /// </summary>
        public Cliente Cliente { get; init; }

        /// <summary>
        /// Local de origem do pedido.
        /// </summary>
        public string Origem { get; set; }

        /// <summary>
        /// Local de destino do pedido.
        /// </summary>
        public string Destino { get; set; }

        /// <summary>
        /// Data de criação do pedido.
        /// </summary>
        public DateTime DataCriacao { get; init; }

    }
}