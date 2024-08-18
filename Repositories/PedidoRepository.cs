using Frenet.ShipManagement.Data;
using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.Repositories.Interface;
using Frenet.ShipManagement.Views.Dto;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace Frenet.ShipManagement.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly FrenetShipManagementContext _context;

        /// <summary>
        /// Inicializa uma nova instância do repositório de clientes.
        /// </summary>
        /// <param name="context">Contexto do banco de dados</param>
        public PedidoRepository(FrenetShipManagementContext context)
        {
            _context = context;
        }

        public async Task<List<Pedido>> GetPedidos()
        {
            var pedidos = await _context.Pedido
             .OrderByDescending(p => p.DataCriacao)
             .Take(10)
             .ToListAsync();

            return pedidos;
        }

        public async Task<Pedido> CreatePedido(PedidoDto pedido)
        {
            var criarPedido = new Pedido
            {
                ClienteId = pedido.ClienteId,
                Origem = pedido.Origem,
                Destino = pedido.Destino,
                Status = pedido.Status,
            };

            _context.Pedido.Add(criarPedido);
            await _context.SaveChangesAsync();

            return criarPedido;
        }

        public async Task<Pedido> GetPedidoById(int id)
        {
            var pedido = await _context.Pedido
            .Include(p => p.Cliente)
            .FirstOrDefaultAsync(m => m.Id == id);

            return pedido;
        }


        public async Task<List<Pedido>> GetPedidosByClienteId(int clienteId)
        {
            var pedidos = await _context.Pedido
                .Include(p => p.Cliente)
                .Where(p => p.ClienteId == clienteId)
                .ToListAsync();
            return pedidos;
        }

        public async Task<Pedido> UpdatePedido(int id, PedidoDto pedido)
        {
            var pedidoExistente = await _context.Pedido.FindAsync(id);

            pedidoExistente.Destino = pedido.Destino;
            pedidoExistente.Origem = pedido.Origem;
            pedidoExistente.Status = pedido.Status;

            _context.Entry(pedidoExistente).CurrentValues.SetValues(pedidoExistente);
            await _context.SaveChangesAsync();

            return pedidoExistente;
        }
    }
}
