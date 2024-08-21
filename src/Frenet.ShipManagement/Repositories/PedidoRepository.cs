using Frenet.ShipManagement.Data;
using Frenet.ShipManagement.DTOs;
using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.Repositories.Interface;
using Frenet.ShipManagement.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Frenet.ShipManagement.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Inicializa uma nova instância do repositório de pedidos.
        /// </summary>
        /// <param name="context">Contexto do banco de dados</param>
        public PedidoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém a lista dos últimos 10 pedidos ordenados pela data de criação.
        /// </summary>
        /// <returns>Lista de objetos PedidoResponse</returns>
        public async Task<List<PedidoResponse>> GetPedidos()
        {
            var pedidos = await _context.Pedido
                .OrderByDescending(p => p.DataCriacao)
                .Take(10)
                .ToListAsync();

            var pedidoResponses = pedidos.Select(pedido => new PedidoResponse
            {
                ClienteId = pedido.ClienteId, // Acessando ClienteId de cada item
                Pedido = new PedidoDto
                {
                    Id = pedido.Id,
                    Origem = pedido.Origem,
                    Destino = pedido.Destino,
                    DataCriacao = pedido.DataCriacao,
                    Status = pedido.Status,
                    ValorFrete = pedido.ValorFrete
                }
            }).ToList();

            return pedidoResponses;
        }

        /// <summary>
        /// Cria um novo pedido e o salva no banco de dados.
        /// </summary>
        /// <param name="pedido">Objeto Pedido a ser criado</param>
        /// <returns>Pedido criado</returns>
        public async Task<Pedido> CreatePedido(Pedido pedido)
        {
            _context.Pedido.Add(pedido);
            await _context.SaveChangesAsync();

            return pedido;
        }

        /// <summary>
        /// Obtém um pedido pelo seu ID.
        /// </summary>
        /// <param name="id">ID do pedido</param>
        /// <returns>Pedido com o ID especificado</returns>
        public async Task<Pedido> GetPedidoById(int id)
        {
            return await _context.Pedido.FirstOrDefaultAsync(m => m.Id == id);
        }

        /// <summary>
        /// Obtém a lista de pedidos associados a um cliente específico.
        /// </summary>
        /// <param name="clienteId">ID do cliente</param>
        /// <returns>Lista de pedidos do cliente</returns>
        public async Task<List<PedidoResponse>> GetPedidosByClienteId(int clienteId)
        {
            var pedidos = await _context.Pedido
                .Where(p => p.ClienteId == clienteId)
                .ToListAsync();

            var pedidoResponses = pedidos.Select(pedido => new PedidoResponse
            {
                ClienteId = pedido.ClienteId,
                Pedido = new PedidoDto
                {
                    Id = pedido.Id,
                    Origem = pedido.Origem,
                    Destino = pedido.Destino,
                    DataCriacao = pedido.DataCriacao,
                    Status = pedido.Status,
                    ValorFrete = pedido.ValorFrete
                }
            }).ToList();

            return pedidoResponses;
        }

        /// <summary>
        /// Atualiza um pedido existente com os dados fornecidos.
        /// </summary>
        /// <param name="id">ID do pedido a ser atualizado</param>
        /// <param name="pedidoDto">Objeto PedidoDto contendo os novos dados</param>
        /// <returns>Pedido atualizado</returns>
        public async Task<Pedido> UpdatePedido(int id, PedidoRequest pedidoDto, decimal frete)
        {
            var pedidoExistente = await _context.Pedido.FindAsync(id);

            if (pedidoExistente == null)
            {
                return null;
            }

            pedidoExistente.Destino = pedidoDto.Destino;
            pedidoExistente.Origem = pedidoDto.Origem;
            pedidoExistente.ValorFrete = frete;

            _context.Pedido.Update(pedidoExistente);
            await _context.SaveChangesAsync();

            return pedidoExistente;
        }
        public async Task<Pedido> UpdateStatus(int id, Status status)
        {
            var pedidoExistente = await _context.Pedido.FindAsync(id);

            if (pedidoExistente == null)
            {
                return null;
            }

            pedidoExistente.Status = status;

            _context.Pedido.Update(pedidoExistente);
            await _context.SaveChangesAsync();

            return pedidoExistente;
        }
        public async Task Remove(Pedido pedido)
        {
            _context.Pedido.Remove(pedido);
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
