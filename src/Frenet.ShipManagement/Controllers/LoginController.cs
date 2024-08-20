using Frenet.ShipManagement.Data;
using Frenet.ShipManagement.DTOs;
using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    public const string UserName = "userName";
    public const string UserId = "userId";
    public const string Permissions = "permissions";
    public const string UserOperators = "userOperators";

    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ApplicationDbContext _context;
    private readonly AuthenticationConfiguration _authConfiguration;

    public LoginController(SignInManager<IdentityUser> signInManager, ApplicationDbContext context, IOptions<AuthenticationConfiguration> authConfiguration)
    {
        _signInManager = signInManager;
        _context = context;
        _authConfiguration = authConfiguration.Value;
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Authenticate([FromBody] UserLoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.Email);

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, true, false);

        if (!result.Succeeded)
        {
            return Unauthorized();
        }

        var now = DateTime.UtcNow;

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(LoginController.UserName, user.UserName),
            new Claim(LoginController.UserId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
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

        return Ok(new
        {
            JwtResponse = encodedJwt,
            Expires = jwtSecurityToken.ValidTo,
        });

    }
}
