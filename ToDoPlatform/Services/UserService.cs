using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using ToDoPlatform.Helpers;
using ToDoPlatform.Models;
using ToDoPlatform.ViewModels;

namespace ToDoPlatform.Services;

public class UserService : IUserService
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<UserService> _logger;

    public UserService(
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager,
        ILogger<UserService> logger
    )
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<SignInResult> Login(LoginVM login)
    {
        string userName = login.Email;
        if (Helper.IsValidEmail(login.Email))
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user != null)
                userName = user.UserName;
        }

        var result = await _signInManager.PasswordSignInAsync(
            userName, login.Password, login.RememberMe, lockoutOnFailure: true
        );

        if (result.Succeeded)
            _logger.LogInformation($"Usuário '{userName}' acessou o sistema");
        if (result.IsLockedOut)
            _logger.LogWarning($"Usuário '{userName}' está bloqueado");
        if (result.IsNotAllowed)
            _logger.LogWarning($"O usuário '{userName}' está tentando acessar uma área restritsa");
        
        return result;
    }

    public async Task Logout()
    {
        _logger.LogInformation($"Usuário '{ClaimTypes.Email}' saiu do sistema");
        await _signInManager.SignOutAsync();
    }
}

public async Task<List<string>> Register(RegisterVM register)
{
    var user = new AppUser()
    {
        Name = register.Name,
        UserName = register.Email,
        NormalizedUserName = register.Email.Normalize(),
        Email = register.Email,
        NormalizedEmail = register.Email.Normalize(),
        EmailConfirmed = true,
        LockoutEnabled = true,
    };

    var addUser = await _userManager.CreateAsync(user, register.Password);

    List<string> result = [];

    if (addUser.Succeeded)
    {
        _logger.LogInformation($"Novo usuário registrado: {register.Email}");
        await _userManager.AddToRoleAsync(user, "Usuário");
    }
    else
    {
        foreach (var error in addUser.Errors)
        {
            result.Add(TranslateIdentityErrors.TranslateErrorMessage(error.Code));
        }
    }

    return result;
}