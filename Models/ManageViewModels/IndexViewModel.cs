using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WonosWebApp.Models.ManageViewModels
{
    public class IndexViewModel
    {
        [DisplayName("Nombre")]
        public string Nombres { get; set; }
        [DisplayName("Apellidos")]
        public string Apellidos { get; set; }
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Numero de Telefono")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
        [DisplayName("DNI o Carnet de Extranjeria")]
        [StringLength(8,MinimumLength =8, ErrorMessage = "El DNI debe tener 8 numeros")]
        public string Dni { get; set; }
        [DisplayName("RUC")]
        [StringLength(11, MinimumLength =11,ErrorMessage = "El RUC debe tener  11 numeros")]
        public string Ruc { get; set; }
        [DisplayName("Tipo de Persona")]
        public string Type { get; set; }
    }
}
