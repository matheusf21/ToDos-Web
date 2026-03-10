
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using ToDoPlatform.ViewModels;

namespace ToDoPlatform.Services;

public class UserService : IUserService
{
    public Task<SignInResult> Login(LoginVM login)
    {
        throw new NotImplementedException();
    }

    public Task Logout()
    {
        throw new NotImplementedException();
    }

    public async Task Logout()
    {
        _logger.LogInformation(
            $"Usuário '{ClaimTypes.Email} saiu do sistema");
        await _signInManager.SignOutAsync();    
    }
}
