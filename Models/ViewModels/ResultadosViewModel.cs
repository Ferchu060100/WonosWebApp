using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WonosWebApp.Models.Resultados;

namespace WonosWebApp.Models.ViewModels
{
    public class ResultadosViewModel
    {
        public Bono bono { get; set; }
        public Estructuracion estructura { get; set; }
        public Rentabilidad rentabilidad { get; set; }
        public Utilidad utilidad { get; set; }
        public List<Periodo> periodos { get; set; }
    }
}
