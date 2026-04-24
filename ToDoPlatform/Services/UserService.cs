using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoPlatform.Data;
using ToDoPlatform.Helpers;
using ToDoPlatform.Models;
using ToDoPlatform.ViewModels;

namespace ToDoPlatform.Services;

public class UserService : IUserService
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<UserService> _logger;
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager,
        ILogger<UserService> logger,
        AppDbContext dbContext,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UserVM> GetLoggedUser()
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return null;

        var user = await _dbContext.AppUsers.SingleOrDefaultAsync(u => u.Id == userId);
        var roles = string.Join(", ", await _userManager.GetRolesAsync(user));
        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

        return new UserVM
        {
            Id = user.Id,
            UserName = user.UserName,
            Name = user.Name,
            Email = user.Email,
            ProfilePicture = user.ProfilePicture,
            Roles = roles,
            IsAdmin = isAdmin
        };        
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
                result.Add(error.Code);
            }
        }

        return result;
    }
}