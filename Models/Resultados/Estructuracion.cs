using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WonosWebApp.Models.Resultados
{
    public class Estructuracion
    {
        public double frecCupon { get; set; }
        public int totalPeriodos { get; set; }
        public double IPer { get; set; }
        public double IA { get; set; }
        public double TEA { get; set; }
        public double TEP { get; set; }
        public double COK { get; set; }
        public double costesInicialesEmisor { get; set; }
        public double costesInicialesBonista { get; set; }
    }
}
