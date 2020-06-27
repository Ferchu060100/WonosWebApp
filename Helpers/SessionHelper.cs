using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WonosWebApp.Models;

namespace WonosWebApp.Helpers
{
    public class SessionHelper
    {
        public static ApplicationUser User { get; set; }
        public static string nombreBono { get; set; }
        public static string tipoMetodo { get; set; }
    }
}
