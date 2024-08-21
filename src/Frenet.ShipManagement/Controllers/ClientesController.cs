using Frenet.ShipManagement.Data;
using Frenet.ShipManagement.DTOs;
using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;

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
        private readonly ApplicationDbContext _context;
        private readonly IClienteService _clienteService;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Inicializa uma nova instância do controlador de clientes.
        /// </summary>
        /// <param name="context">Contexto do banco de dados</param>
        /// <param name="clienteService">Serviço de clientes</param>
        public ClientesController(ApplicationDbContext context, IClienteService clienteService)
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
            Logger.Info("Iniciando a recuperação da lista de clientes.");

            try
            {
                var clientes = await _clienteService.GetClientesAsync();
                Logger.Info("Lista de clientes recuperada com sucesso.");
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao obter clientes.");
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
                Logger.Warn("ID inválido fornecido: {ID}", id);
                return BadRequest("ID inválido.");
            }

            Logger.Info("Iniciando a recuperação dos detalhes do cliente com ID: {ID}", id);

            try
            {
                var cliente = await _clienteService.GetClienteById(id);
                if (cliente == null)
                {
                    Logger.Warn("Cliente com ID {ID} não encontrado.", id);
                    return NotFound();
                }

                Logger.Info("Detalhes do cliente com ID {ID} recuperados com sucesso.", id);
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao obter detalhes do cliente com ID: {ID}", id);
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
                Logger.Warn("Dados inválidos para criação de cliente: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            Logger.Info("Iniciando a criação do cliente.");

            try
            {
                await _clienteService.CreateCliente(clienteDto);
                Logger.Info("Cliente criado com sucesso.");
                return Ok("Cliente salvo");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao criar cliente.");
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
                Logger.Warn("Dados inválidos para atualização de cliente: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            Logger.Info("Iniciando a atualização do cliente com ID: {ID}", id);

            try
            {
                var clienteUpdate = await _clienteService.UpdateCliente(cliente, id);
                if (clienteUpdate == null)
                {
                    Logger.Warn("Cliente com ID {ID} não encontrado para atualização.", id);
                    return NotFound($"Cliente com ID {id} não encontrado.");
                }

                Logger.Info("Cliente com ID {ID} atualizado com sucesso.", id);
                return Ok($"Cliente atualizado: {clienteUpdate.Id}");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Logger.Error(ex, "Erro ao atualizar cliente com ID: {ID}", id);
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
                Logger.Warn("ID inválido fornecido para exclusão: {ID}", id);
                return BadRequest("ID inválido.");
            }

            Logger.Info("Iniciando a exclusão do cliente com ID: {ID}", id);

            try
            {
                var cliente = await _context.Cliente.FindAsync(id);
                if (cliente == null)
                {
                    Logger.Warn("Cliente com ID {ID} não encontrado para exclusão.", id);
                    return NotFound("Cliente não encontrado.");
                }

                await _clienteService.DeleteCliente(id);
                Logger.Info("Cliente com ID {ID} excluído com sucesso.", id);
                return Ok($"Cliente {id} deletado");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao excluir cliente com ID: {ID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao excluir cliente.");
            }
        }
    }
}
