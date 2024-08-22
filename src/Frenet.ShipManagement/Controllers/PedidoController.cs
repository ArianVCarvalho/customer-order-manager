using Frenet.ShipManagement.Data;
using Frenet.ShipManagement.DTOs;
using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.Services.Interface;
using Frenet.ShipManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Frenet.ShipManagement.Controllers
{
    /// <summary>
    /// Controlador responsável por operações relacionadas a pedidos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PedidoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPedidoService _pedidoService;
        private readonly IClienteService _clienteService;
        private readonly ILogger<PedidoController> _logger;

        /// <summary>
        /// Inicializa uma nova instância de PedidoController.
        /// </summary>
        /// <param name="context">Contexto do banco de dados para operações relacionadas aos pedidos.</param>
        /// <param name="pedidoService">Serviço de pedidos que encapsula a lógica de negócio.</param>
        /// <param name="clienteService">Serviço de clientes que encapsula a lógica de negócio relacionada aos clientes.</param>
        /// <param name="logger">Logger para registro de eventos e erros.</param>
        public PedidoController(
            ApplicationDbContext context,
            IPedidoService pedidoService,
            IClienteService clienteService,
            ILogger<PedidoController> logger
        )
        {
            _context = context;
            _pedidoService = pedidoService;
            _clienteService = clienteService;
            _logger = logger;
        }

        /// <summary>
        /// Obtém os 10 pedidos mais recentes.
        /// </summary>
        /// <returns>Uma lista de pedidos, incluindo os dados de clientes associados.</returns>
        /// <response code="200">Retorna a lista de pedidos.</response>
        /// <response code="401">Acesso não autorizado.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PedidoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPedidos(CancellationToken cancellation)
        {
            try
            {
                var pedidos = await _pedidoService.GetPedidos(cancellation);
                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter pedidos.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter pedidos.");
            }
        }

        /// <summary>
        /// Obtém os detalhes de um pedido específico pelo ID.
        /// </summary>
        /// <param name="id">O ID do pedido.</param>
        /// <returns>Os detalhes do pedido, incluindo os dados do cliente associado.</returns>
        /// <response code="200">Retorna os detalhes do pedido.</response>
        /// <response code="401">Acesso não autorizado.</response>
        /// <response code="404">Pedido não encontrado.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(PedidoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var pedido = await _pedidoService.GetPedidoById(id);

                if (pedido == null)
                {
                    _logger.LogWarning("Pedido com ID: {Id} não encontrado.", id);
                    return NotFound("Pedido não encontrado.");
                }

                return Ok(pedido);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter detalhes do pedido com ID: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter detalhes do pedido.");
            }
        }

        /// <summary>
        /// Obtém um pedido pelo ID do cliente associado.
        /// </summary>
        /// <param name="id">O ID do cliente.</param>
        /// <returns>O pedido relacionado ao cliente.</returns>
        /// <response code="200">Retorna o pedido associado ao cliente.</response>
        /// <response code="401">Acesso não autorizado.</response>
        /// <response code="404">Nenhum pedido encontrado para o cliente.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpGet("{id:int}/cliente")]
        [ProducesResponseType(typeof(IEnumerable<PedidoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPedidosByClienteId(int id)
        {
            try
            {
                var cliente = await _clienteService.GetClienteById(id);

                if (cliente == null)
                {
                    _logger.LogWarning("Cliente com ID: {Id} não encontrado.", id);
                    return NotFound("ID do cliente não foi encontrado.");
                }

                var pedidos = await _pedidoService.GetPedidosByClienteId(id);

                if (pedidos == null || pedidos.Count == 0)
                {
                    _logger.LogWarning("Nenhum pedido encontrado para o cliente com ID: {Id}", id);
                    return NotFound("Nenhum pedido encontrado para o cliente com o ID fornecido.");
                }

                _logger.LogInformation("Pedidos para o cliente com ID: {Id} obtidos com sucesso.", id);
                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter pedidos para o cliente com ID: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter pedidos do cliente.");
            }
        }

        /// <summary>
        /// Cria um novo pedido.
        /// </summary>
        /// <param name="pedido">O DTO contendo os dados do pedido a ser criado.</param>
        /// <returns>O pedido criado.</returns>
        /// <response code="200">Retorna o pedido criado.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="401">Acesso não autorizado.</response>
        /// <response code="404">Cliente não encontrado.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpPost]
        [ProducesResponseType(typeof(PedidoRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] PedidoRequest pedido)
        {
            try
            {
                var cliente = await _clienteService.GetClienteById(pedido.ClienteId);

                if (cliente == null)
                {
                    _logger.LogWarning("Cliente com ID: {ClienteId} não encontrado.", pedido.ClienteId);
                    return NotFound("Cliente nao encontrado.");
                }

                if (ModelState.IsValid)
                {
                    await _pedidoService.CreatePedido(pedido);
                    return Ok(pedido);
                }

                _logger.LogWarning("Dados inválidos para criação do pedido.");
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar pedido.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao criar pedido.");
            }
        }

        /// <summary>
        /// Atualiza um pedido existente.
        /// </summary>
        /// <param name="id">O ID do pedido a ser atualizado.</param>
        /// <param name="pedido">O DTO contendo os dados atualizados do pedido.</param>
        /// <returns>O pedido atualizado.</returns>
        /// <response code="200">Retorna o pedido atualizado.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="401">Acesso não autorizado.</response>
        /// <response code="404">Pedido não encontrado.</response>
        /// <response code="409">Concorrência detectada.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(PedidoRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Edit(int id, [FromBody] PedidoRequest pedido)
        {
            try
            {
                var isPedido = await _pedidoService.GetPedidoById(id);

                if (isPedido == null)
                {
                    _logger.LogWarning("Pedido com ID: {Id} não encontrado.", id);
                    return NotFound("Pedido não existe.");
                }

                if (ModelState.IsValid)
                {
                    await _pedidoService.UpdatePedido(id, pedido);
                    _logger.LogInformation("Pedido com ID: {Id} atualizado com sucesso.", id);
                    return Ok(pedido);
                }

                _logger.LogWarning("Dados inválidos para atualização do pedido com ID: {Id}", id);
                return BadRequest(ModelState);
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogError("Houve um problema com a atualização do pedido com ID: {Id}. O pedido pode ter sido modificado por outro usuário.", id);
                return Conflict("Houve um problema com a atualização. O pedido pode ter sido modificado por outro usuário.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o pedido com ID: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar a solicitação.");
            }
        }

        /// <summary>
        /// Atualiza o status de um pedido existente.
        /// </summary>
        /// <param name="id">O ID do pedido a ser atualizado.</param>
        /// <param name="status">O novo status do pedido.</param>
        /// <returns>Uma resposta indicando o sucesso ou falha da atualização do status.</returns>
        /// <response code="200">Status atualizado com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="401">Acesso não autorizado.</response>
        /// <response code="404">Pedido não encontrado.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpPut("{id:int}/status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] Status status)
        {
            try
            {
                var pedido = await _pedidoService.GetPedidoById(id);

                if (pedido == null)
                {
                    _logger.LogWarning("Pedido com ID: {Id} não encontrado.", id);
                    return NotFound("Pedido não encontrado.");
                }

                pedido.Status = status;
                await _pedidoService.UpdateStatus(id, status);

                return Ok($"Status do pedido {pedido.Id} atualizado para {pedido.Status}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar status do pedido com ID: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao atualizar status do pedido.");
            }
        }

        /// <summary>
        /// Exclui um pedido pelo ID.
        /// </summary>
        /// <param name="id">O ID do pedido a ser excluído.</param>
        /// <returns>Uma resposta indicando o sucesso ou falha da exclusão.</returns>
        /// <response code="200">Exclusão bem-sucedida.</response>
        /// <response code="401">Acesso não autorizado.</response>
        /// <response code="404">Pedido não encontrado.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var pedido = await _pedidoService.GetPedidoById(id);

                if (pedido == null)
                {
                    _logger.LogWarning("Pedido com ID: {Id} não encontrado.", id);
                    return NotFound("Pedido não encontrado.");
                }

                await _pedidoService.Remove(pedido);

                _logger.LogInformation("Pedido com ID: {Id} deletado com sucesso.", id);
                return Ok($"Pedido {pedido.Id} deletado com sucesso");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir pedido com ID: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao excluir pedido.");
            }
        }
    }
}
