using Frenet.ShipManagement.Data;
using Frenet.ShipManagement.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.ViewModels;
using NLog;

public class AuthService : IAuthService
{
    public const string UserName = "userName";
    public const string UserId = "userId";
    public const string Permissions = "permissions";
    public const string UserOperators = "userOperators";

    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ApplicationDbContext _context;
    private readonly AuthenticationConfiguration _authConfiguration;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public AuthService(SignInManager<IdentityUser> signInManager, ApplicationDbContext context, IOptions<AuthenticationConfiguration> authConfiguration)
    {
        _signInManager = signInManager;
        _context = context;
        _authConfiguration = authConfiguration.Value;
    }

    public async Task<IActionResult> AuthenticateAsync(UserLoginRequest request)
    {
        Logger.Info("Iniciando o processo de autenticação para o usuário: {Email}", request.Email);

        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.Email);

            if (user == null)
            {
                Logger.Warn("Usuário não encontrado: {Email}", request.Email);
                return new UnauthorizedResult(); // User not found
            }

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, true, false);

            if (!result.Succeeded)
            {
                Logger.Warn("Falha na autenticação para o usuário: {Email}", request.Email);
                return new UnauthorizedResult(); // Password incorrect
            }

            var now = DateTime.UtcNow;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID
                new Claim(UserName, user.UserName), // Custom claim for username
                new Claim(UserId, user.Id.ToString()), // Custom claim for user ID
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64) // Issued At
            };

            var symmetricKeyAsBase64 = _authConfiguration.Secret;
            var keyByteArray = Encoding.UTF8.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _authConfiguration.Issuer,
                audience: _authConfiguration.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(_authConfiguration.ExpiresInMinutes)),
                signingCredentials: signingCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            Logger.Info("Autenticação bem-sucedida para o usuário: {Email}", request.Email);
            return new OkObjectResult(new
            {
                JwtResponse = encodedJwt,
                Expires = jwtSecurityToken.ValidTo,
            });
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Erro ao processar a solicitação de autenticação para o usuário: {Email}", request.Email);
            return new StatusCodeResult(500); // Internal server error
        }
    }
}
