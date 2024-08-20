namespace Frenet.ShipManagement.Models
{
    /// <summary>
    /// Representa o status de um pedido no sistema de gestão de envios.
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// O pedido está em processamento.
        /// </summary>
        Processamento = 1,

        /// <summary>
        /// O pedido foi enviado.
        /// </summary>
        Enviado = 2,

        /// <summary>
        /// O pedido foi entregue.
        /// </summary>
        Entregue = 3,

        /// <summary>
        /// O pedido foi cancelado.
        /// </summary>
        Cancelado = 4,
    }
}
