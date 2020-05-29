using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WonosWebApp.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<long>
    {
        [DisplayName("Tipo de Persona")]
        public string Type { get; set; }
        [DisplayName("DNI o Carnet de Extranjeria")]
        [StringLength(8, ErrorMessage = "El numero debe tener como maximo 8 caracteres")]
        public string Dni { get; set; }
    }
}
