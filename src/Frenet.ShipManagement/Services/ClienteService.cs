using Frenet.ShipManagement.DTOs;
using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.Repositories.Interface;
using Frenet.ShipManagement.Services.Interface;
using Microsoft.Extensions.Logging;

namespace Frenet.ShipManagement.Services
{
    /// <summary>
    /// Implementação do serviço de gestão de clientes.
    /// </summary>
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repository;
        private readonly ILogger<ClienteService> _logger;

        /// <summary>
        /// Inicializa uma nova instância do serviço de clientes.
        /// </summary>
        /// <param name="repository">Repositório de clientes</param>
        /// <param name="logger">Logger para registrar eventos e erros</param>
        public ClienteService(IClienteRepository repository, ILogger<ClienteService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Obtém a lista de todos os clientes.
        /// </summary>
        /// <returns>Uma lista de clientes</returns>
        public async Task<List<Cliente>> GetClientesAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Obtendo a lista de todos os clientes.");
            var clientes = await _repository.GetClientesAsync(cancellationToken);
            _logger.LogInformation("Lista de clientes obtida com sucesso. Total de clientes: {Count}", clientes.Count);
            return clientes;
        }

        /// <summary>
        /// Obtém um cliente específico pelo ID.
        /// </summary>
        /// <param name="id">ID do cliente</param>
        /// <returns>O cliente correspondente ao ID</returns>
        /// <exception cref="KeyNotFoundException">Lançado quando o cliente não é encontrado</exception>
        public async Task<IResult> GetClienteById(int id)
        {
            _logger.LogInformation("Obtendo cliente pelo ID: {Id}", id);
            var cliente = await _repository.GetClienteById(id);

            if (cliente == null)
            {
                _logger.LogWarning("Cliente nao encontrado. ID: {Id}", id);
                return Results.NotFound<string>("Cliente não encontrado");
            }

            _logger.LogInformation("Cliente encontrado. ID: {Id}", id);
            return Results.Ok<Cliente>(cliente);
        }

        /// <summary>
        /// Cria um novo cliente com base nos dados fornecidos.
        /// </summary>
        /// <param name="cliente">Dados do cliente a ser criado</param>
        /// <returns>O cliente criado</returns>
        public async Task<Cliente> CreateCliente(ClienteDto cliente)
        {
            _logger.LogInformation("Criando novo cliente. Nome: {Nome}, Email: {Email}", cliente.Nome, cliente.Email);
            var novoCliente = await _repository.CreateCliente(cliente);
            _logger.LogInformation("Cliente criado com sucesso. ID: {Id}", novoCliente.Id);
            return novoCliente;
        }

        /// <summary>
        /// Atualiza as informações de um cliente existente.
        /// </summary>
        /// <param name="cliente">Dados atualizados do cliente</param>
        /// <param name="id">ID do cliente a ser atualizado</param>
        /// <returns>O cliente atualizado</returns>
        public async Task<Cliente> UpdateCliente(ClienteDto cliente, int id)
        {
            _logger.LogInformation("Atualizando cliente. ID: {Id}", id);
            var clienteAtualizado = await _repository.UpdateCliente(cliente, id);
            _logger.LogInformation("Cliente atualizado com sucesso. ID: {Id}", id);
            return clienteAtualizado;
        }

        public async Task DeleteCliente(int id)
        {
            _logger.LogInformation("Removendo cliente. ID: {Id}", id);
            await _repository.DeleteCliente(id);
            _logger.LogInformation("Cliente removido com sucesso. ID: {Id}", id);
        }
    }
}
