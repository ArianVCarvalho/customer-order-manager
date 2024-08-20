using Azure.Core;
using Frenet.ShipManagement.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Frenet.ShipManagement.Services;

public class UserIdentitySeedService : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    // Define constants for user details
    private const string DefaultEmail = "uncle.bob@frenet.com.br";
    private const string DefaultPassword = "QK8uZ*ZorO46";

    public UserIdentitySeedService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();
        var userStore = scope.ServiceProvider.GetService<IUserStore<IdentityUser>>();
        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        // Check if the user already exists
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == DefaultEmail);

        if (user != null)
        {
            return;
        }

        user = new IdentityUser();

        // Set user details
        await userStore.SetUserNameAsync(user, DefaultEmail, CancellationToken.None);
        await userManager.CreateAsync(user, DefaultPassword);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
