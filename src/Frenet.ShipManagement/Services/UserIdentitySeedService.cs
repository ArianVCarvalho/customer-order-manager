using Azure.Core;
using Frenet.ShipManagement.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Frenet.ShipManagement.Services
{
    public class UserIdentitySeedService : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<UserIdentitySeedService> _logger;

        // Define constantes para os detalhes do usuário
        private const string DefaultEmail = "uncle.bob@frenet.com.br";
        private const string DefaultPassword = "QK8uZ*ZorO46";

        public UserIdentitySeedService(IServiceScopeFactory serviceScopeFactory, ILogger<UserIdentitySeedService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando o processo de seeding de identidade de usuário...");

            using var scope = _serviceScopeFactory.CreateScope();
            var userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();
            var userStore = scope.ServiceProvider.GetService<IUserStore<IdentityUser>>();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            // Verifica se o usuário já existe
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == DefaultEmail);

            if (user != null)
            {
                return;
            }

            user = new IdentityUser();

            try
            {
                // Define os detalhes do usuário
                await userStore.SetUserNameAsync(user, DefaultEmail, CancellationToken.None);
                await userManager.CreateAsync(user, DefaultPassword);
                _logger.LogInformation("Usuário com o e-mail {Email} criado com sucesso.", DefaultEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao criar o usuário com o e-mail {Email}.", DefaultEmail);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Parando o serviço de seeding de identidade de usuário.");
            return Task.CompletedTask;
        }
    }
}
