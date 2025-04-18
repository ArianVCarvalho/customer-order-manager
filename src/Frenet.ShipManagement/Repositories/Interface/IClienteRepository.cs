﻿using Frenet.ShipManagement.DTOs;
using Frenet.ShipManagement.Models;

namespace Frenet.ShipManagement.Repositories.Interface
{
    /// <summary>
    /// Interface que define os métodos para acesso e manipulação de dados de clientes.
    /// </summary>
    public interface IClienteRepository
    {
        /// <summary>
        /// Cria um novo cliente com base nos dados fornecidos.
        /// </summary>
        /// <param name="cliente">Dados do cliente a ser criado</param>
        /// <returns>O cliente criado</returns>
        Task<Cliente> CreateCliente(ClienteDto cliente);
        Task DeleteCliente(int id);

        /// <summary>
        /// Obtém um cliente específico pelo ID.
        /// </summary>
        /// <param name="id">ID do cliente</param>
        /// <returns>O cliente correspondente ao ID</returns>
        Task<Cliente> GetClienteById(int id);

        /// <summary>
        /// Obtém a lista de todos os clientes.
        /// </summary>
        /// <returns>Uma lista de clientes</returns>
        Task<List<Cliente>> GetClientesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Atualiza as informações de um cliente existente.
        /// </summary>
        /// <param name="cliente">Dados atualizados do cliente</param>
        /// <returns>O cliente atualizado</returns>
        Task<Cliente> UpdateCliente(ClienteDto cliente, int id);
    }
}
