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
        [StringLength(8, ErrorMessage = "El numero debe tener como maximo 8 caracteres")]
        public string Dni { get; set; }
        [DisplayName("RUC")]
        [StringLength(11, ErrorMessage = "El RUC debe tener como maximo 11 caracteres")]
        public string Ruc { get; set; }
        [DisplayName("Tipo de Persona")]
        public string Type { get; set; }
    }
}
