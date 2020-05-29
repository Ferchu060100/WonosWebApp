using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WonosWebApp.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [DisplayName("DNI o Carnet de Extranjeria")]
        [StringLength(8, ErrorMessage = "El numero debe tener como maximo 8 caracteres")]
        public string Dni { get; set; }
        [DisplayName("Tipo de Persona")]
        public string Type { get; set; }
        [DisplayName("Numero de Telefono")]
        public string PhoneNumber { get; set; }

    }
}
