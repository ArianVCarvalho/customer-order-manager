using Frenet.ShipManagement.DTOs;
using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.Repositories.Interface;
using Frenet.ShipManagement.Services.Interface;

namespace Frenet.ShipManagement.Services
{
    /// <summary>
    /// Implementação do serviço de gestão de clientes.
    /// </summary>
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository repository;

        /// <summary>
        /// Inicializa uma nova instância do serviço de clientes.
        /// </summary>
        /// <param name="repository">Repositório de clientes</param>
        public ClienteService(IClienteRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Obtém a lista de todos os clientes.
        /// </summary>
        /// <returns>Uma lista de clientes</returns>
        public async Task<List<Cliente>> GetClientesAsync()
        {
            return await repository.GetClientesAsync();
        }

        /// <summary>
        /// Obtém um cliente específico pelo ID.
        /// </summary>
        /// <param name="id">ID do cliente</param>
        /// <returns>O cliente correspondente ao ID</returns>
        /// <exception cref="KeyNotFoundException">Lançado quando o cliente não é encontrado</exception>
        public async Task<IResult> GetClienteById(int id)
        {
            var cliente = await repository.GetClienteById(id);
            if (cliente == null)
            {
                return Results.NotFound<string>("Cliente não encontrado");
            }
            return Results.Ok<Cliente>(cliente);
        }

        /// <summary>
        /// Cria um novo cliente com base nos dados fornecidos.
        /// </summary>
        /// <param name="cliente">Dados do cliente a ser criado</param>
        /// <returns>O cliente criado</returns>
        public async Task<Cliente> CreateCliente(ClienteDto cliente)
        {
            return await repository.CreateCliente(cliente);
        }

        /// <summary>
        /// Atualiza as informações de um cliente existente.
        /// </summary>
        /// <param name="cliente">Dados atualizados do cliente</param>
        /// <returns>O cliente atualizado</returns>
        public async Task<Cliente> UpdateCliente(ClienteDto cliente, int id)
        {
            return await repository.UpdateCliente(cliente, id);
        }

        public async Task DeleteCliente(int id)
        {
            await repository.DeleteCliente(id);
        }
    }
}
