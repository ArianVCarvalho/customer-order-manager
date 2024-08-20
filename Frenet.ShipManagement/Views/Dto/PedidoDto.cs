using Frenet.ShipManagement.Models;

namespace Frenet.ShipManagement.Views.Dto
{
    /// <summary>
    /// Representa os dados transferidos de um pedido no sistema de gestão de envios.
    /// Esta classe é utilizada para transferir informações de um pedido entre camadas da aplicação
    /// e para facilitar a comunicação com a interface do usuário ou outras APIs.
    /// </summary>
    public class PedidoDto
    {
        /// <summary>
        /// Identificador do pedido.
        /// </summary>
        public int Id { get; set; }
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

        /// <summary>
        /// Data de criação do pedido.
        /// Representa a data e hora em que o pedido foi criado no sistema.
        /// </summary>
        public DateTime DataCriacao { get; set; }

        /// <summary>
        /// Valor Total do Pedido.
        /// Representa o custo total do frete para este pedido, calculado com base em peso, dimensões e destino.
        /// </summary>
        public decimal ValorFrete { get; set; }
    }
}
