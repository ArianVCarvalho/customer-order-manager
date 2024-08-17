using Frenet.ShipManagement.Data;
using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.Repositories.Interface;
using Frenet.ShipManagement.Views.Dto;
using Microsoft.EntityFrameworkCore;

namespace Frenet.ShipManagement.Repositories
{
    /// <summary>
    /// Implementação do repositório de clientes.
    /// </summary>
    public class ClienteRepository : IClienteRepository
    {
        private readonly FrenetShipManagementContext _context;

        /// <summary>
        /// Inicializa uma nova instância do repositório de clientes.
        /// </summary>
        /// <param name="context">Contexto do banco de dados</param>
        public ClienteRepository(FrenetShipManagementContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém a lista de todos os clientes.
        /// </summary>
        /// <returns>Uma lista de clientes</returns>
        public async Task<List<Cliente>> GetClientesAsync()
        {
            return await _context.Cliente.ToListAsync();
        }

        /// <summary>
        /// Obtém um cliente específico pelo ID.
        /// </summary>
        /// <param name="id">ID do cliente</param>
        /// <returns>O cliente correspondente ao ID</returns>
        public async Task<Cliente> GetClienteById(int id)
        {
            var cliente = await _context.Cliente.FirstOrDefaultAsync(x => x.Id == id);
            return cliente;
        }

        /// <summary>
        /// Cria um novo cliente com base nos dados fornecidos.
        /// </summary>
        /// <param name="cliente">Dados do cliente a ser criado</param>
        /// <returns>O cliente criado</returns>
        async Task<Cliente> IClienteRepository.CreateCliente(ClienteDto cliente)
        {
            var criarCliente = new Cliente
            {
                Nome = cliente.Nome,
                Telefone = cliente.Telefone,
                Endereco = cliente.Endereco,
                Email = cliente.Email,
            };

            _context.Add(criarCliente);
            await _context.SaveChangesAsync();

            return criarCliente;
        }

        /// <summary>
        /// Atualiza as informações de um cliente existente.
        /// </summary>
        /// <param name="cliente">Dados atualizados do cliente</param>
        /// <returns>O cliente atualizado</returns>
        async Task<Cliente> IClienteRepository.UpdateCliente(ClienteDto cliente)
        {
            var updateCliente = new Cliente
            {
                Nome = cliente.Nome,
                Telefone = cliente.Telefone,
                Endereco = cliente.Endereco,
                Email = cliente.Email,
            };

            _context.Cliente.Update(updateCliente);
            await _context.SaveChangesAsync();

            return updateCliente;
        }
    }
}
