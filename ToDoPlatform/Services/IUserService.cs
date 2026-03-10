using Microsoft.AspNetCore.Identity;
using ToDoPlatform.Models;
using ToDoPlatform.ViewModels;

namespace ToDoPlatform.Services;

public interface IUserService
{
    public IUserService(
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager,
        ILogger<UserService> logger
     )
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }
    Task<SignInResult> Login(LoginVM login);
    Task Logout();
}
