using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoPlatform.Models;
using ToDoPlatform.MOdels;

namespace ToDoPlatform.Data;

public class AppDbSeed
{
    public AppDbSeed(ModelBuilder builder)
    {
        #region Popular dados de Perfil de Usuário
        List<IdentityRole> roles = new()
        {
            new()
            {
                Id = "7c1ba6b2-0cbf-424d-b0d0-891a95f6a58a",
                Name = "Administrador",
                NormalizedName = "ADMINISTRADOR",
            }, 
            new()
            {
                Id = "e0b3ec4f-ece6-44f7-a7fc-f3587f824e52",
                Name = "Usuário",
                NormalizedName = "USUÁRIO",
            }, 
        };
        builder.Entity<IdentityRole>().HasData(roles);
        #endregion

        #region Popular dados de Usuário
        List<AppUser> users = new()
        {
            new ()
            {
                Id = "4ed00981-5260-4b2f-b5b3-f8236d02efb5",
                Email = "matheusferreiradelima21@gmail.com",
                NormalizedEmail = "MATHEUSFERREIRADELIMA21@GMAIL.COM",
                UserName = "matheusferreiradelima21@gmail.com",
                NormalizedUserName = "MATHEUSFERREIRADELIMA21@GMAIL.COM",
                LockoutEnabled = false,
                EmailConfirmed = true,
                Name = "Matheus Ferreira de Lima",
                ProfilePicture = "/img/users/4ed00981-5260-4b2f-b5b3-f8236d02efb5",
            },
            new ()
            {
                Id = "19878399-84da-48de-8132-407af7cff87a",
                Email = "thayllasanatana672@gmail.com",
                NormalizedEmail = "THAYLLASANTANA672@GMAIL.COM",
                UserName = "thayllasantana672@gmail.com",
                NormalizedUserName = "THAYLLASANTANA672@GMAIL.COM",
                LockoutEnabled = false,
                EmailConfirmed = true,
                Name = "Thaylla Santana Pereira",
                ProfilePicture = "/img/users/19878399-84da-48de-8132-407af7cff87a",
            },
        };

        foreach (var user in users)
        {
            PasswordHasher<AppUser> pass = new();
            user.PasswordHash = pass.HashPassword(user, "123456");
        }

        builder.Entity<AppUser>().HasData(users);
        #endregion

        #region Popular Dados de Usuário Perfil
        List<IdentityUserRole<string>> userRoles = new()
        {
            new IdentityUserRole<string>()
            {
                UserId = users[0].Id,
                RoleId = roles[0].Id
            },
            new IdentityUserRole<string>()
            {
                UserId = users[1].Id,
                RoleId = roles[1].Id
            },
        };

        builder.Entity<IdentityUserRole<string>>().HasData(userRoles);
        #endregion

        #region Popular ToDos
        List<ToDo> toDos = new()
        {
            new ToDo()
            {
                Id = 1,
                Title = "Ler livro do Seminário",
                Description = "Finalizar até 27/03",
                UserId = users[0].Id
            },
        };

        builder.Entity<ToDo>().HasData(toDos);
        #endregion
    }
}