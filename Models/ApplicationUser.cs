﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WonosWebApp.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [DisplayName("Nombre")]
        public string Nombres { get; set; }
        [DisplayName("Apellidos")]
        public string Apellidos { get; set; }

        [DisplayName("Tipo de Persona")]
        public string Type { get; set; }
        [DisplayName("DNI o Carnet de Extranjeria")]
        [StringLength(8, ErrorMessage = "El DNI debe 8 caracteres")]
        public string Dni { get; set; }
        [DisplayName("RUC")]
        [StringLength(11, ErrorMessage = "El RUC debe tener  11 caracteres")]
        public string Ruc { get; set; }

    }
}
