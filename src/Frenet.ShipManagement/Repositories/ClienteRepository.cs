using Frenet.ShipManagement.Data;
using Frenet.ShipManagement.DTOs;
using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Frenet.ShipManagement.Repositories
{
    /// <summary>
    /// Implementação do repositório de clientes.
    /// </summary>
    public class ClienteRepository : IClienteRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Inicializa uma nova instância do repositório de clientes.
        /// </summary>
        /// <param name="context">Contexto do banco de dados</param>
        public ClienteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém a lista de todos os clientes.
        /// </summary>
        /// <returns>Uma lista de clientes</returns>
        public async Task<List<Cliente>> GetClientesAsync(CancellationToken cancellationToken)
        {
            return await _context.Cliente.ToListAsync(cancellationToken);
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
        public async Task<Cliente> CreateCliente(ClienteDto cliente)
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
        public async Task<Cliente> UpdateCliente(ClienteDto cliente, int id)
        {
            var updateCliente = await _context.Cliente.FindAsync(id);

            if (updateCliente == null)
                throw new KeyNotFoundException("Cliente não encontrado");

            updateCliente.Nome = cliente.Nome;
            updateCliente.Telefone = cliente.Telefone;
            updateCliente.Endereco = cliente.Endereco;
            updateCliente.Email = cliente.Email;

            _context.Cliente.Update(updateCliente);
            await _context.SaveChangesAsync();

            return updateCliente;
        }

        public async Task DeleteCliente(int id)
        {
            var cliente = await GetClienteById(id);

            if (cliente == null)
            {
                throw new KeyNotFoundException("Cliente não encontrado.");
            }

            _context.Cliente.Remove(cliente);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbUpdateConcurrencyException("Falha na exclusão devido a uma modificação concorrente. Tente novamente.", ex);
            }
        }
    }
}
