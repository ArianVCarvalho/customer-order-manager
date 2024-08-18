using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Frenet.ShipManagement.Data;
using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.Views.Dto;
using Frenet.ShipManagement.Services.Interface;

namespace Frenet.ShipManagement.Controllers
{
    /// <summary>
    /// Controlador responsável por operações relacionadas a pedidos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly FrenetShipManagementContext _context;
        private readonly IPedidoService _pedidoService;
        private readonly IClienteService _clienteService;

        /// <summary>
        /// Inicializa uma nova instância de PedidoController.
        /// </summary>
        /// <param name="context">Contexto do banco de dados para operações relacionadas aos pedidos.</param>
        /// <param name="pedidoService">Serviço de pedidos que encapsula a lógica de negócio.</param>
        /// <param name="clienteService">Serviço de clientes que encapsula a lógica de negócio relacionada aos clientes.</param>
        public PedidoController(
            FrenetShipManagementContext context,
            IPedidoService pedidoService,
            IClienteService clienteService
        )
        {
            _context = context;
            _pedidoService = pedidoService;
            _clienteService = clienteService;
        }

        /// <summary>
        /// Obtém todos os pedidos.
        /// </summary>
        /// <returns>Uma lista de pedidos, incluindo os dados de clientes associados.</returns>
        /// <response code="200">Retorna a lista de pedidos.</response>
        [HttpGet]
        public async Task<IActionResult> GetPedidos()
        {
            var pedidos = await _context.Pedido.Include(p => p.Cliente).ToListAsync();
            return Ok(pedidos);
        }

        /// <summary>
        /// Obtém os detalhes de um pedido específico pelo ID.
        /// </summary>
        /// <param name="id">O ID do pedido.</param>
        /// <returns>Os detalhes do pedido, incluindo os dados do cliente associado.</returns>
        /// <response code="200">Retorna os detalhes do pedido.</response>
        /// <response code="404">Se o pedido não for encontrado.</response>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var pedido = await _pedidoService.GetPedidoById(id);

            if (pedido == null)
            {
                return NotFound("Pedido não encontrado.");
            }

            return Ok(pedido);
        }

        /// <summary>
        /// Obtém um pedido pelo ID do cliente associado.
        /// </summary>
        /// <param name="id">O ID do cliente.</param>
        /// <returns>O pedido relacionado ao cliente.</returns>
        /// <response code="200">Retorna o pedido associado ao cliente.</response>
        /// <response code="404">Se nenhum pedido/cliente for encontrado para o cliente.</response>
        [HttpGet("{id:int}/cliente")]
        public async Task<IActionResult> GetPedidosByClienteId(int id)
        {
            var cliente = await _clienteService.GetClienteById(id);

            if (cliente == null)
            {
                return NotFound("ID do cliente não foi encontrado.");
            }

            var pedido = await _pedidoService.GetPedidosByClienteId(id);

            if (pedido == null || pedido.Count == 0)
            {
                return NotFound("Nenhum pedido encontrado para o cliente com o ID fornecido.");
            }

            return Ok(pedido);
        }

        /// <summary>
        /// Cria um novo pedido.
        /// </summary>
        /// <param name="pedido">O DTO contendo os dados do pedido a ser criado.</param>
        /// <returns>O pedido criado.</returns>
        /// <response code="200">Retorna o pedido criado.</response>
        /// <response code="400">Se os dados do pedido não forem válidos.</response>
        /// <response code="404">Se o cliente associado ao pedido não for encontrado.</response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PedidoDto pedido)
        {
            var cliente = await _clienteService.GetClienteById(pedido.ClienteId);

            if (ModelState.IsValid)
            {
                await _pedidoService.CreatePedido(pedido);
                return Ok(pedido);
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Atualiza um pedido existente.
        /// </summary>
        /// <param name="id">O ID do pedido a ser atualizado.</param>
        /// <param name="pedido">O DTO contendo os dados atualizados do pedido.</param>
        /// <returns>O pedido atualizado.</returns>
        /// <response code="200">Retorna o pedido atualizado.</response>
        /// <response code="400">Se os dados do pedido não forem válidos ou o ID não corresponder.</response>
        /// <response code="404">Se o pedido não for encontrado.</response>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Edit(int id, [FromBody] PedidoDto pedido)
        {
            var isPedido = await _pedidoService.GetPedidoById(id);

            if (isPedido == null)
            {
                return NotFound("Pedido não existe.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Atualiza o pedido
                    await _pedidoService.UpdatePedido(id, pedido);
                    return Ok(pedido);
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Exceção lançada em caso de concorrência
                    return Conflict("Houve um problema com a atualização. O pedido pode ter sido modificado por outro usuário.");
                }
                catch (Exception ex)
                {
                    // Captura e loga a exceção genérica
                    // LogException(ex); // Método de logging hipotético
                    return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar a solicitação.");
                }
            }

            return Ok($"{pedido} atualizado com sucesso.");
        }
        /// <summary>
        /// Exclui um pedido pelo ID.
        /// </summary>
        /// <param name="id">O ID do pedido a ser excluído.</param>
        /// <returns>Uma resposta indicando o sucesso ou falha da exclusão.</returns>
        /// <response code="200">Se a exclusão for bem-sucedida.</response>
        /// <response code="404">Se o pedido não for encontrado.</response>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var pedido = await _context.Pedido.FindAsync(id);

            if (pedido != null)
            {
                _context.Pedido.Remove(pedido);
                await _context.SaveChangesAsync();
                return Ok($"Pedido {pedido.Id} deletado com sucesso");
            }

            return NotFound("Pedido não encontrado.");
        }

        /// <summary>
        /// Verifica se um pedido existe no banco de dados.
        /// </summary>
        /// <param name="id">O ID do pedido.</param>
        /// <returns>Verdadeiro se o pedido existe, falso caso contrário.</returns>
        private bool PedidoExists(int id)
        {
            return _context.Pedido.Any(e => e.Id == id);
        }
    }
}
