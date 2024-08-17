using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.Views.Dto;

namespace Frenet.ShipManagement.Services.Interface
{
    /// <summary>
    /// Define os contratos para a gestão de clientes.
    /// </summary>
    public interface IClienteService
    {
        /// <summary>
        /// Cria um novo cliente com base nos dados fornecidos.
        /// </summary>
        /// <param name="cliente">Dados do cliente a ser criado.</param>
        /// <returns>O cliente criado.</returns>
        Task<Cliente> CreateCliente(ClienteDto cliente);

        /// <summary>
        /// Obtém um cliente com base no ID fornecido.
        /// </summary>
        /// <param name="id">ID do cliente.</param>
        /// <returns>O cliente correspondente ao ID fornecido.</returns>
        Task<Cliente> GetClienteById(int id);

        /// <summary>
        /// Obtém a lista de todos os clientes.
        /// </summary>
        /// <returns>Uma lista de todos os clientes.</returns>
        Task<List<Cliente>> GetClientesAsync();

        /// <summary>
        /// Atualiza os dados de um cliente existente com base nas informações fornecidas.
        /// </summary>
        /// <param name="cliente">Dados atualizados do cliente.</param>
        /// <returns>O cliente atualizado.</returns>
        Task<Cliente> UpdateCliente(ClienteDto cliente);
    }
}
