using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WonosWebApp.Models.Resultados
{
    public class Periodo
    {
        public int N { get; set; }
        public DateTime FechaPago {get;set;}
        public string plazoGracia { get; set; }
        public double? bono { get; set; }
        public double? bonoindexado { get; set; }
        public double? inflacionA { get; set; }
        public double inflacionPer { get; set; }
        public double? cupon { get; set; }
        public double? cuota { get; set; }
        public double? amortizacion { get; set; }
        public double? prima { get; set; }
        public double? escudo { get; set; }
        public double flujoEmisor { get; set; }
        public double? flujoEmisorEscudo { get; set; }
        public double flujoBonista { get; set; }

    }
}
