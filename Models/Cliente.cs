using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Frenet.ShipManagement.Models
{
    /// <summary>
    /// Representa um cliente no sistema de gestão de envios.
    /// </summary>
    public class Cliente
    {
        /// <summary>
        /// Identificador único do cliente.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Nome do cliente.
        /// </summary>
        public string Nome { get; init; }

        /// <summary>
        /// Endereço físico do cliente.
        /// </summary>
        public string Endereco { get; init; }

        /// <summary>
        /// Número de telefone de contato do cliente.
        /// </summary>
        public string Telefone { get; init; }

        /// <summary>
        /// Endereço de e-mail do cliente.
        /// </summary>
        public string Email { get; init; }

        /// <summary>
        /// Lista de pedidos associados a este cliente.
        /// </summary>
        [JsonIgnore]
        public ICollection<Pedido> Pedidos { get; init; }
    }
}
