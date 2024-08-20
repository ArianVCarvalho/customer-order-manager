namespace Frenet.ShipManagement.DTOs
{
    /// <summary>
    /// Representa a simulação com dados de origem e destino.
    /// </summary>
    public class SimulacaoDto
    {
        /// <summary>
        /// Identificador da origem para a simulação.
        /// </summary>
        public string Origem { get; set; }

        /// <summary>
        /// Identificador do destino para a simulação.
        /// </summary>
        public string Destino { get; set; }
    }
}