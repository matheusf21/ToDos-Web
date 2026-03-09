
using System.ComponentModel.DataAnnotations;

namespace ToDoPlatform.ViewModels;

public class LoginVM
    {
        [Display(Name = "E-mail", Prompt = "seu@gmail.com")]
        [Required(ErrorMessage = "O e-mail de acesso é obrigatório!")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido!")]
        public string Email {get; set; }

        [DataType(DataType.Password)]
        public string Password {get; set; }
        public bool RememberMe {get; set; }
        public string ReturnUrl{get; set; }
    }

