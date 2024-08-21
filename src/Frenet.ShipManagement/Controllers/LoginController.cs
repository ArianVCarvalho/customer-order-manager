using Frenet.ShipManagement.DTOs;
using Frenet.ShipManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Threading.Tasks;

namespace Frenet.ShipManagement.Controllers
{
    /// <summary>
    /// Controlador responsável pela autenticação de usuários.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authService;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Inicializa uma nova instância de LoginController.
        /// </summary>
        /// <param name="authService">Serviço de autenticação que encapsula a lógica de login.</param>
        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Autentica um usuário e retorna um token JWT se as credenciais forem válidas.
        /// </summary>
        /// <param name="request">Dados de login do usuário.</param>
        /// <returns>Um token JWT para o usuário autenticado.</returns>
        /// <response code="200">Retorna um token JWT se as credenciais forem válidas.</response>
        /// <response code="401">Credenciais inválidas.</response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public Task<IActionResult> Authenticate([FromBody] UserLoginRequest request)
        {
            Logger.Info("Autenticando usuário com o email de usuário: {UserName}", request.Email);
            return _authService.AuthenticateAsync(request);
        }
    }
}
