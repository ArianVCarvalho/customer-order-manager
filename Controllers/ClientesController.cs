using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Frenet.ShipManagement.Data;
using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.Services.Interface;
using Frenet.ShipManagement.Views.Dto;

namespace Frenet.ShipManagement.Controllers
{
    /// <summary>
    /// Controlador responsável por gerenciar clientes.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly FrenetShipManagementContext _context;
        private readonly IClienteService _clienteService;

        /// <summary>
        /// Inicializa uma nova instância do controlador de clientes.
        /// </summary>
        /// <param name="context">Contexto do banco de dados</param>
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
        public async Task<IActionResult> Index()
        {
            return Ok(await _clienteService.GetClientesAsync());
        }

        /// <summary>
        /// Retorna os detalhes de um cliente específico.
        /// </summary>
        /// <param name="id">ID do cliente</param>
        /// <returns>Detalhes do cliente</returns>
        [HttpGet("{id:int}/detalhe")]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _clienteService.GetClienteById(id);
            if (cliente == null)
            {
                return NotFound();
            }

            return Ok(cliente);
        }

        /// <summary>
        /// Cria um novo cliente.
        /// </summary>
        /// <param name="clienteDto">Dados do cliente</param>
        /// <returns>Resultado da criação</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ClienteDto clienteDto)
        {
            if (ModelState.IsValid)
            {
                var cliente = _clienteService.CreateCliente(clienteDto);

                //TODO: Entender serialização do retorno
                return Ok("Usuário salvo");
            }
            return BadRequest();
        }

        /// <summary>
        /// Atualiza os dados de um cliente existente.
        /// </summary>
        /// <param name="id">ID do cliente</param>
        /// <param name="cliente">Dados atualizados do cliente</param>
        /// <returns>Resultado da atualização</returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Edit(int id, [FromBody] ClienteDto cliente)
        {
            Cliente clienteUpdate = null;

            if (ModelState.IsValid)
            {

                try
                {
                    clienteUpdate = await _clienteService.UpdateCliente(cliente, id);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new Exception("Erro ao salvar");
                }
            }
            return Ok($"Usuário atualizado: {clienteUpdate.Id}");
        }

        /// <summary>
        /// Exclui um cliente existente.
        /// </summary>
        /// <param name="id">ID do cliente a ser excluído</param>
        /// <returns>Resultado da exclusão</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {

            if (id == null || id == 0)
            {
                return BadRequest("ID inválido.");
            }

            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente != null)
            {

                await _clienteService.DeleteCliente(id);
                return Ok($"Usuário {id} deletado");

            }
            return NotFound("Cliente não existe.");

            
        }
    }
}
