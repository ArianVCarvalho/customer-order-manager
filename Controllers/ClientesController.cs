using Frenet.ShipManagement.Data;
using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.Services.Interface;
using Frenet.ShipManagement.Views.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Frenet.ShipManagement.Controllers
{
    /// <summary>
    /// Controlador responsável por gerenciar clientes.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientesController : ControllerBase
    {
        private readonly FrenetShipManagementContext _context;
        private readonly IClienteService _clienteService;

        /// <summary>
        /// Inicializa uma nova instância do controlador de clientes.
        /// </summary>
        /// <param name="context">Contexto do banco de dados</param>
        /// <param name="clienteService">Serviço de clientes</param>
        public ClientesController(FrenetShipManagementContext context, IClienteService clienteService)
        {
            _context = context;
            _clienteService = clienteService;
        }

        /// <summary>
        /// Retorna a lista de todos os clientes.
        /// </summary>
        /// <returns>Lista de clientes</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ClienteDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Index()
        {
            try
            {
                var clientes = await _clienteService.GetClientesAsync();
                return Ok(clientes);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter clientes.");
            }
        }

        /// <summary>
        /// Retorna os detalhes de um cliente específico.
        /// </summary>
        /// <param name="id">ID do cliente</param>
        /// <returns>Detalhes do cliente</returns>
        [HttpGet("{id:int}/detalhe")]
        [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID inválido.");
            }

            try
            {
                var cliente = await _clienteService.GetClienteById(id);
                if (cliente == null)
                {
                    return NotFound();
                }

                return Ok(cliente);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter detalhes do cliente.");
            }
        }

        /// <summary>
        /// Cria um novo cliente.
        /// </summary>
        /// <param name="clienteDto">Dados do cliente</param>
        /// <returns>Resultado da criação</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] ClienteDto clienteDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _clienteService.CreateCliente(clienteDto);
                return Ok("Cliente salvo");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao criar cliente.");
            }
        }

        /// <summary>
        /// Atualiza os dados de um cliente existente.
        /// </summary>
        /// <param name="id">ID do cliente</param>
        /// <param name="cliente">Dados atualizados do cliente</param>
        /// <returns>Resultado da atualização</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Edit(int id, [FromBody] ClienteDto cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var clienteUpdate = await _clienteService.UpdateCliente(cliente, id);
                if (clienteUpdate == null)
                {
                    return NotFound($"Cliente com ID {id} não encontrado.");
                }

                return Ok($"Cliente atualizado: {clienteUpdate.Id}");
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao atualizar cliente.");
            }
        }

        /// <summary>
        /// Exclui um cliente existente.
        /// </summary>
        /// <param name="id">ID do cliente a ser excluído</param>
        /// <returns>Resultado da exclusão</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID inválido.");
            }

            try
            {
                var cliente = await _context.Cliente.FindAsync(id);
                if (cliente == null)
                {
                    return NotFound("Cliente não encontrado.");
                }

                await _clienteService.DeleteCliente(id);
                return Ok($"Cliente {id} deletado");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao excluir cliente.");
            }
        }
    }
}
