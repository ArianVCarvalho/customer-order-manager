using System;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Xunit;
using FluentAssertions;

namespace Frenet.ShipManagement.UnitTests.Services
{
    public class TokenServiceTests
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly TokenService _tokenService;

        public TokenServiceTests()
        {
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.SetupGet(config => config["Jwt:Key"]).Returns("supersecretkey123456supersecretkey123456"); // Use a longer key
            _configurationMock.SetupGet(config => config["Jwt:Issuer"]).Returns("Frenet.ShipManagement");
            _configurationMock.SetupGet(config => config["Jwt:Audience"]).Returns("Frenet.ShipManagementClient");

            _tokenService = new TokenService(_configurationMock.Object);
        }

        [Fact]
        public void GenerateToken_ShouldReturnAValidJwtToken()
        {
            var token = _tokenService.GenerateToken();

            token.Should().NotBeNullOrEmpty();

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            jsonToken.Should().NotBeNull();
            jsonToken.Issuer.Should().Be("Frenet.ShipManagement");
            jsonToken.Audiences.Should().Contain("Frenet.ShipManagementClient");
            jsonToken.ValidTo.Should().BeAfter(DateTime.UtcNow);
            jsonToken.ValidFrom.Should().BeBefore(DateTime.UtcNow);
        }

        [Fact]
        public void GenerateToken_ShouldIncludeClaims()
        {
            var token = _tokenService.GenerateToken();

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            jsonToken.Should().NotBeNull();
            var claims = jsonToken?.Claims;

            claims.Should().Contain(claim => claim.Type == JwtRegisteredClaimNames.Sub && claim.Value == "defaultClientIdentifier");
            claims.Should().Contain(claim => claim.Type == JwtRegisteredClaimNames.Jti);
        }

        [Fact]
        public void GenerateToken_ShouldThrowException_WhenKeyIsMissing()
        {
            _configurationMock.SetupGet(config => config["Jwt:Key"]).Returns((string)null);

            Action act = () => new TokenService(_configurationMock.Object);

            act.Should().Throw<ArgumentException>()
                .WithMessage("JWT Key não configurada. (Parameter 'key')");
        }
    }
}