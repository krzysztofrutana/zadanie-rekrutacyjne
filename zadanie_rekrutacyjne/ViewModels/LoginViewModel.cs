using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace zadanie_rekrutacyjne.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Login nie może być pusty.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Hasło nie może być puste.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name ="Zapamiętaj mnie")]
        public bool RememberMe { get; set; }
    }
}
