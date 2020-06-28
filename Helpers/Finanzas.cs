using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using WonosWebApp.Models;
using WonosWebApp.Models.Resultados;

namespace WonosWebApp.Helpers
{
    public class Finanzas
    {
        public static double HallarTEA(double TNP, int diasAño, int diascapitalización)
        {
            double m = diasAño / diascapitalización;
            return Math.Round(Math.Pow(1 + (TNP / m), m) - 1, 9);
        }
        public static double HallarTEP( double Tasa, int diasAño, int diascapitalizacion)
        {
            double fraccion = (double)diascapitalizacion / diasAño;
            return Math.Round(Math.Pow(1 + Tasa, fraccion) - 1, 9);
        }
        public static double HallarCOKPeriodo(double tasaDescuento, int diasAño, int frecuenciaCupon)
        {
            double COKPeriodo;
            COKPeriodo = Math.Round(Math.Pow(1 + tasaDescuento, frecuenciaCupon / diasAño) - 1, 9);
            return COKPeriodo;
        }
         public static double CIEmisor(double estructuracion,double colocacion,double flotacion,double cavali,double VComercial)
        {
            double CIEmi = (estructuracion  + colocacion + cavali) * VComercial;
            return CIEmi;
        }

        public static double CIBonista(double flotacion, double cavali,double VComercial)
        {
            double CIBon = (flotacion + cavali) * VComercial;
            return CIBon;

        }
        public static double HallarCuota(double TasaPeriodo,double Saldo,double Amortizacion)
        {
            double Cuota = Saldo * TasaPeriodo/100 + Amortizacion;
            return Cuota;
        }

        public static double HallarRenta(double bono, double TEP, int numeroCuotas)
        {
            return Math.Round(bono * (Math.Pow(1 + TEP, numeroCuotas) * TEP) / (Math.Pow(1 + TEP, numeroCuotas) - 1), 2);
        }





        public static Estructuracion ResultadosEstructuracion(Bono bono)
        {
            return new Estructuracion
            {
                totalPeriodos = (bono.diasporanio / bono.frecuencia) * bono.nroaños,
                TEA = bono.tipoTasa == "Efectiva" ? bono.tasaInteres : HallarTEA(bono.tasaInteres, bono.diasporanio, bono.capitalizacion),
                TEP = HallarTEP(bono.tasaInteres, bono.diasporanio, bono.capitalizacion),
                COK = Math.Round(Math.Pow(1 + bono.tasaDescuentoCOK, bono.frecuencia / bono.diasporanio) - 1, 9),
                frecCupon = bono.frecuencia,
                costesInicialesBonista = CIBonista(bono.pFlota, bono.pCAVALI, bono.vcomercial),
                costesInicialesEmisor = CIEmisor(bono.pEstructura,bono.pColoca,bono.pFlota,bono.pCAVALI,bono.vcomercial)
            };

        }
        public static List<Periodo> ResultadosPeriodosAmericano (Bono bono, Estructuracion estructuracion)
        {
            List<Periodo> lista = new List<Periodo>();
            Periodo cero = new Periodo
            {
                N = 0,
                plazoGracia = null,
                bono = 0,
                cupon = 0,
                amortizacion = 0,
                prima = 0,
                escudo = 0,
                flujoEmisor= bono.vcomercial- estructuracion.costesInicialesEmisor,
                flujoEmisorEscudo = bono.vcomercial - estructuracion.costesInicialesEmisor,
                flujoBonista = bono.vcomercial - estructuracion.costesInicialesBonista
            };
            lista.Add(cero);
            for (int i = 0; i < estructuracion.totalPeriodos; i++)
            {
                Periodo aux = new Periodo();
                aux.N = i + 1;
                aux.plazoGracia = null;
                aux.bono = i == 0 ? bono.vnominal : Math.Round(lista[i].bono.Value - lista[i].amortizacion.Value, 2);
                aux.cupon = Math.Round(aux.bono.Value * estructuracion.TEP/100, 2);
                aux.amortizacion = aux.N == estructuracion.totalPeriodos - 1 ? 0.00 : aux.bono.Value;
                aux.cuota = HallarCuota(estructuracion.TEP, aux.bono.Value, aux.amortizacion.Value);
                aux.prima = aux.N == estructuracion.totalPeriodos - 1 ? 0.00 : Math.Round(aux.bono.Value * bono.pPrima,2);
                aux.escudo = Math.Round(aux.cuota.Value * bono.impRenta,2);
                aux.flujoEmisor = Math.Round(aux.cuota.Value + aux.prima.Value,2);
                aux.flujoEmisorEscudo = Math.Round(aux.flujoEmisor - aux.escudo.Value, 2);
                aux.flujoBonista = aux.flujoEmisor;

                lista.Add(aux);
            }
            return lista;
        }

        public static List<Periodo> ResultadosPeriodosAleman(Bono bono, Estructuracion estructuracion)
        {
            List<Periodo> lista = new List<Periodo>();
            Periodo cero = new Periodo
            {
                N = 0,
                plazoGracia = null,
                bono = null,
                cupon = null,
                amortizacion = null,
                prima = null,
                escudo = null,
                flujoEmisor = bono.vcomercial - estructuracion.costesInicialesEmisor,
                flujoEmisorEscudo = bono.vcomercial - estructuracion.costesInicialesEmisor,
                flujoBonista = bono.vcomercial - estructuracion.costesInicialesBonista
            };
            lista.Add(cero);
            double amortizacion = bono.vcomercial / estructuracion.totalPeriodos;
            for (int i = 0; i < estructuracion.totalPeriodos; i++)
            {
                Periodo aux = new Periodo();
                aux.N = i + 1;
                aux.plazoGracia = null;
                aux.bono = i == 0 ? bono.vnominal : Math.Round(lista[i - 1].bono.Value - lista[i - 1].amortizacion.Value, 2);
                aux.cupon = i == 0 ? 0 : Math.Round(aux.bono.Value * estructuracion.TEP, 2);
                aux.amortizacion = amortizacion;
                aux.cuota = HallarCuota(estructuracion.TEP, aux.bono.Value, aux.amortizacion.Value);
                aux.prima = aux.N == estructuracion.totalPeriodos - 1 ? 0.00 : Math.Round(aux.bono.Value * bono.pPrima, 2);
                aux.escudo = Math.Round(aux.cuota.Value * bono.impRenta, 2);
                aux.flujoEmisor = Math.Round(aux.cuota.Value + aux.prima.Value, 2);
                aux.flujoEmisorEscudo = Math.Round(aux.flujoEmisor - aux.escudo.Value, 2);
                aux.flujoBonista = aux.flujoEmisor;

                lista.Add(aux);
            }
            return lista;
        }

        public static List<Periodo> ResultadosPeriodosFrances(Bono bono, Estructuracion estructuracion)
        {
            List<Periodo> lista = new List<Periodo>();
            Periodo cero = new Periodo
            {
                N = 0,
                plazoGracia = null,
                bono = null,
                cupon = null,
                amortizacion = null,
                prima = null,
                escudo = null,
                flujoEmisor = bono.vcomercial - estructuracion.costesInicialesEmisor,
                flujoEmisorEscudo = bono.vcomercial - estructuracion.costesInicialesEmisor,
                flujoBonista = bono.vcomercial - estructuracion.costesInicialesBonista
            };
            lista.Add(cero);
            for (int i = 0; i < estructuracion.totalPeriodos; i++)
            {
                Periodo aux = new Periodo();
                aux.N = i + 1;
                aux.plazoGracia = null;
                aux.bono = i == 0 ? bono.vnominal : Math.Round(lista[i - 1].bono.Value - lista[i - 1].amortizacion.Value, 2);
                aux.cupon = i == 0 ? 0 : Math.Round(aux.bono.Value * estructuracion.TEP, 2);
                aux.cuota = HallarRenta(aux.bono.Value, estructuracion.TEP, estructuracion.totalPeriodos - aux.N + 1);
                aux.amortizacion = Math.Round(aux.cuota.Value - aux.cupon.Value,2);
                aux.prima = aux.N == estructuracion.totalPeriodos - 1 ? 0.00 : Math.Round(aux.bono.Value * bono.pPrima, 2);
                aux.escudo = Math.Round(aux.cuota.Value * bono.impRenta, 2);
                aux.flujoEmisor = Math.Round(aux.cuota.Value + aux.prima.Value, 2);
                aux.flujoEmisorEscudo = Math.Round(aux.flujoEmisor - aux.escudo.Value, 2);
                aux.flujoBonista = aux.flujoEmisor;

                lista.Add(aux);
            }
            return lista;
        }

        public static double HallarVAN(Estructuracion estructuracion ,List<Periodo> resultados)
        {
            double VAN;
            double[] flujo = null;
            for (int i = 1; i < resultados.Count(); i++)
            {
                flujo.Append(resultados[i].flujoBonista);
            }
            VAN = Financial.NPV(estructuracion.COK,  ref flujo);
            return VAN;
        }
        public static double HallarUtilidad(double VAN,List<Periodo> resultados)
        {
            double Utilidad;
            Utilidad = -resultados[0].flujoBonista + VAN;
            return Utilidad;

        }

        public static double HallarTCEAEmisor(Estructuracion estructuracion,List<Periodo> resultados,Bono bono)
        {
            double TIR;
            double TCEA;
            double[] flujo = new double[estructuracion.totalPeriodos];
            for (int i = 0; i < resultados.Count(); i++)
            {
                if(i == 0)
                {
                    flujo.Append(resultados[i].flujoEmisor);
                }
                else
                {
                    flujo.Append(-resultados[i].flujoEmisor);
                }
                
            }
            TIR = Financial.IRR(ref flujo);
            TCEA = Math.Pow(TIR + 1, bono.diasporanio / estructuracion.frecCupon) - 1;
            return TCEA;
        }

        public static double HallarTCEAEmisorEscudo(Estructuracion estructuracion, List<Periodo> resultados, Bono bono)
        {
            double TIR;
            double TCEA;
            double[] flujo = null;
            for (int i = 0; i < resultados.Count(); i++)
            {
                if (i == 0)
                {
                    flujo.Append(resultados[i].flujoEmisorEscudo.Value);
                }
                else
                {
                    flujo.Append(-resultados[i].flujoEmisorEscudo.Value);
                }

            }
            TIR = Financial.IRR(ref flujo);
            TCEA = Math.Pow(TIR + 1, bono.diasporanio / estructuracion.frecCupon) - 1;
            return TCEA;
        }

        public static double HallarTREABonista(Estructuracion estructuracion, List<Periodo> resultados, Bono bono)
        {
            double TIR;
            double TCEA;
            double[] flujo = null;
            for (int i = 0; i < resultados.Count(); i++)
            {
                if (i == 0)
                {
                    flujo.Append(-resultados[i].flujoEmisorEscudo.Value);
                }
                else
                {
                    flujo.Append(resultados[i].flujoEmisorEscudo.Value);
                }

            }
            TIR = Financial.IRR(ref flujo);
            TCEA = Math.Pow(TIR + 1, bono.diasporanio / estructuracion.frecCupon) - 1;
            return TCEA;
        }

        public static Rentabilidad ResultadosRentabilidad(Estructuracion estructuracion,List<Periodo>resultados, Bono bono)
        {
            return new Rentabilidad
            {
                TCEAEmisor = HallarTCEAEmisor(estructuracion, resultados, bono),
                TCEAEmisorEscudo =HallarTCEAEmisorEscudo(estructuracion,resultados,bono),
                TREABonista = HallarTREABonista(estructuracion,resultados,bono)

            };

        }

        public static Utilidad ResultadosUtilidad(Estructuracion estructuracion, List<Periodo> periodos)
        {
            return new Utilidad
            {
                precioActual = HallarVAN(estructuracion, periodos),
                utilidad = HallarUtilidad(HallarVAN(estructuracion, periodos), periodos)

            };
        }



    }
}
