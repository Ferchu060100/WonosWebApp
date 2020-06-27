using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WonosWebApp.Models
{
    public class Bono
    {
        public int Id { get; set; }
        public double vnominal { get; set; }
        public double vcomercial { get; set; }
        public int nroaños { get; set; }
        public int frecuencia { get; set; }
        public int diasporanio { get; set; }
        public int capitalizacion { get; set; }
        public string tipoTasa { get; set; }
        public double tasaInteres { get; set; }
        public double tasaDescuentoCOK { get; set; }
        public double impRenta { get; set; }
        public System.DateTime fechaEmision { get; set; }
        //Gastos Iniciales
        public double pPrima { get; set; }

        public double pEstructura { get; set; }

        public double pColoca { get; set; }

        public double pFlota { get; set; }

        public double pCAVALI { get; set; }
        public string TipoMetodo { get; set; }
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
