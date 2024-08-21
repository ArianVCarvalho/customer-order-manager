using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Frenet.ShipManagement.DTOs;
using Frenet.ShipManagement.ViewModels;

public interface IAuthService
{
    Task<IActionResult> AuthenticateAsync(UserLoginRequest request);
}
