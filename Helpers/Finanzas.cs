using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
            return Math.Round(Math.Pow( Tasa, fraccion), 9);
        }

  

        public static double HallarRenta(double bono, double TEP, int numeroCuotas)
        {
            return Math.Round(bono * (Math.Pow(1 + TEP, numeroCuotas) * TEP) / (Math.Pow(1 + TEP, numeroCuotas) - 1), 2);
        }





        public static Estructuracion ResultadosEstructuracion(Bono bono)
        {
            double TEAr = bono.tipoTasa == "Efectiva" ? bono.tasaInteres : HallarTEA(bono.tasaInteres, bono.diasporanio, bono.capitalizacion);
            double exponente = (double)bono.frecuencia / bono.diasporanio;
            double tea = Math.Round(Math.Pow(1 + TEAr/100,exponente), 7) - 1 ;
            double Ip = Math.Round(Math.Pow(1 + bono.Inflacion/100, exponente), 7) - 1;

            return new Estructuracion
            {
                totalPeriodos = (bono.diasporanio / bono.frecuencia) * bono.nroaños,

                TEA = bono.tipoTasa == "Efectiva" ? bono.tasaInteres : HallarTEA(bono.tasaInteres, bono.diasporanio, bono.capitalizacion),
                TEP = Math.Round(tea * 100,9),
                COK = Math.Round(Math.Pow( bono.tasaDescuentoCOK, (double)bono.frecuencia / bono.diasporanio), 7),
                frecCupon = bono.frecuencia,
                IA = Math.Round(bono.Inflacion,2),
                IPer= Math.Round(Ip *100,3),
                costesInicialesBonista = Math.Round(-(bono.pFlota+bono.pCAVALI)/100*bono.vcomercial,2),
                costesInicialesEmisor =  Math.Round(Math.Round(bono.pEstructura + bono.pFlota+ bono.pCAVALI+ bono.pColoca,7)/100*bono.vcomercial,2)

            };
        }
        public static List<Periodo> ResultadosPeriodosAmericano (Bono bono, Estructuracion estructuracion)
        {
            List<Periodo> lista = new List<Periodo>();
            Periodo cero = new Periodo
            {
                N = 0,
                plazoGracia = "S",
                bono = 0,
                cupon = 0,
                amortizacion = 0,
                bonoindexado = 0,
                cuota = 0,
                prima = 0,
                escudo = 0,
                flujoEmisor= (double)bono.vcomercial - estructuracion.costesInicialesEmisor,
                flujoEmisorEscudo = bono.vcomercial - estructuracion.costesInicialesEmisor,
                flujoBonista = bono.vcomercial - estructuracion.costesInicialesBonista
            };
            lista.Add(cero);

            for (int i = 0 ; i < estructuracion.totalPeriodos; i++)
            {
                Periodo aux = new Periodo();
                aux.N = i + 1;
                if (i < bono.PlazoGraciaCant)
                {
                    aux.plazoGracia = "T";
                }
                else
                {
                    aux.plazoGracia = "S";
                }
                if (aux.plazoGracia == "T")
                {
                    aux.bono = i == 0 ? bono.vnominal : Math.Round(lista[i].bonoindexado.Value + lista[i].cupon.Value, 3);
                    double inf = aux.bono.Value * estructuracion.IPer/100;
                    aux.bonoindexado = inf + aux.bono.Value;
                    
                }else if(lista[i].plazoGracia == "T")
                {
                    aux.bono = Math.Round(lista[i].bonoindexado.Value + lista[i].cupon.Value, 3);
                    double inf = aux.bono.Value * estructuracion.IPer / 100;
                    aux.bonoindexado = Math.Round(inf + aux.bono.Value,2);
                }
                else
                {
                    aux.bono = i == 0? bono.vnominal: Math.Round(lista[i].bonoindexado.Value - lista[i].amortizacion.Value, 3);
                    double inf = aux.bono.Value * estructuracion.IPer / 100;
                    aux.bonoindexado = Math.Round(inf + aux.bono.Value,2);
                }
                aux.cupon = Math.Round(aux.bonoindexado.Value * (estructuracion.TEP)/100, 2);

                if (aux.plazoGracia =="T" )
                {
                    aux.amortizacion = 0;
                }
                else
                {
                    aux.amortizacion = aux.N == estructuracion.totalPeriodos ? aux.bonoindexado.Value : 0.00;

                }
                if (aux.plazoGracia == "T")
                {
                    aux.cuota = 0;
                }
                else
                {
                    aux.cuota = aux.cupon.Value + aux.amortizacion.Value;
                }
                
                aux.prima = aux.N == estructuracion.totalPeriodos ? Math.Round(aux.bono.Value * bono.pPrima/100, 2) : 0.00 ;
                aux.escudo = Math.Round(aux.cupon.Value * bono.impRenta/100,2);
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
                plazoGracia = "S",
                bono = 0,
                cupon = 0,
                amortizacion = 0,
                bonoindexado = 0,
                cuota = 0,
                prima = 0,
                escudo = 0,
                flujoEmisor = (double)bono.vcomercial - estructuracion.costesInicialesEmisor,
                flujoEmisorEscudo = bono.vcomercial - estructuracion.costesInicialesEmisor,
                flujoBonista = bono.vcomercial - estructuracion.costesInicialesBonista
            };
            lista.Add(cero);

            //double amortizacionC = Math.Round(bono.vnominal / estructuracion.totalPeriodos,2);
            for (int i = 0; i < estructuracion.totalPeriodos; i++)
            {
                Periodo aux = new Periodo();
                aux.N = i + 1;
                if (i < bono.PlazoGraciaCant)
                {
                    aux.plazoGracia = "T";
                }
                else
                {
                    aux.plazoGracia = "S";
                }
                if (aux.plazoGracia == "T")
                {
                    aux.bono = i == 0 ? bono.vnominal : Math.Round(lista[i].bonoindexado.Value + lista[i].cupon.Value, 3);
                    double inf = aux.bono.Value * estructuracion.IPer / 100;
                    aux.bonoindexado = inf + aux.bono.Value;

                }
                else if (lista[i].plazoGracia == "T")
                {
                    aux.bono = Math.Round(lista[i].bonoindexado.Value + lista[i].cupon.Value, 3);
                    double inf = aux.bono.Value * estructuracion.IPer / 100;
                    aux.bonoindexado = Math.Round(inf + aux.bono.Value, 2);
                }
                else
                {
                    aux.bono = i == 0 ? bono.vnominal : Math.Round(lista[i].bonoindexado.Value - lista[i].amortizacion.Value, 3);
                    double inf = aux.bono.Value * estructuracion.IPer / 100;
                    aux.bonoindexado = Math.Round(inf + aux.bono.Value, 2);
                }
                aux.cupon = Math.Round(aux.bonoindexado.Value * (estructuracion.TEP) / 100, 2);

                if (aux.plazoGracia == "T")
                {
                    aux.amortizacion = 0;
                }else if(aux.plazoGracia == "S" && i > bono.PlazoGraciaCant )
                        { 
                            int indice = bono.PlazoGraciaCant + 1;
                            int cantPer = estructuracion.totalPeriodos - bono.PlazoGraciaCant;
                            aux.amortizacion = Math.Round(lista[indice].bonoindexado.Value/cantPer,2);
                }
                    else if(aux.plazoGracia == "S" && i ==bono.PlazoGraciaCant)
                    {
                    int cantPer = estructuracion.totalPeriodos - bono.PlazoGraciaCant;
                    aux.amortizacion = Math.Round( aux.bonoindexado.Value / cantPer,2) ;
                    }
                    else
                    {

                    aux.amortizacion = Math.Round(bono.vnominal / estructuracion.totalPeriodos, 2);

                    }
                if (aux.plazoGracia == "T")
                {
                    aux.cuota = 0;
                }
                else
                {
                    aux.cuota = Math.Round(aux.cupon.Value + aux.amortizacion.Value,2);
                }
                aux.prima = aux.N == estructuracion.totalPeriodos ? Math.Round(aux.bono.Value * bono.pPrima / 100, 2) : 0.00;
                aux.escudo = Math.Round(aux.cupon.Value * bono.impRenta / 100, 2);
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
                bono = 0,
                cupon = 0,
                amortizacion = 0,
                cuota = 0,
                prima = 0,
                escudo = 0,
                flujoEmisor = (double)bono.vcomercial - estructuracion.costesInicialesEmisor,
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
                double Arriba = bono.vnominal* estructuracion.TEP/100;
                double Abajo = 1 - Math.Pow(1 + estructuracion.TEP / 100, -estructuracion.totalPeriodos);
                aux.cupon = Math.Round(aux.bono.Value * (estructuracion.TEP) / 100, 2);
                aux.cuota = Math.Round(Arriba / Abajo, 2);
                aux.amortizacion = Math.Round(aux.cuota.Value - aux.cupon.Value, 2);
                aux.prima = aux.N == estructuracion.totalPeriodos ? Math.Round(aux.bono.Value * bono.pPrima / 100, 2) : 0.00;
                aux.escudo = Math.Round(aux.cupon.Value * bono.impRenta / 100, 2);
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
            List<double> flujo = new List<double>();
            for (int i = 1; i < resultados.Count(); i++)
            {
                flujo.Add(resultados[i].flujoBonista);
            }
            var arrFlujos = flujo.ToArray();
            VAN = Math.Round(Financial.NPV(estructuracion.COK/100,  ref arrFlujos),2);
            return VAN;
        }
        public static double HallarUtilidad(double VAN,List<Periodo> resultados)
        {
            double Utilidad;
            Utilidad = Math.Round(-resultados[0].flujoBonista + VAN,2);
            return Utilidad;

        }

        public static double HallarTCEAEmisor(Estructuracion estructuracion,List<Periodo> resultados,Bono bono)
        {
            double TIR;
            double TCEA;
            List<double> flujos = new List<double>();
            for (int i = 0; i < resultados.Count(); i++)
            {
                if(i == 0)
                {
                    flujos.Add(resultados[i].flujoEmisor);
                }
                else
                {
                    flujos.Add(-resultados[i].flujoEmisor);
                }
                
            }
            var arrFlujos = flujos.ToArray();
            TIR = Financial.IRR(ref arrFlujos);
            TCEA = Math.Round( Math.Pow(TIR + 1, bono.diasporanio / estructuracion.frecCupon) - 1,7);
            return TCEA;
        }

        public static double HallarTCEAEmisorEscudo(Estructuracion estructuracion, List<Periodo> resultados, Bono bono)
        {
            double TIR;
            double TCEA;
            List<double> flujos = new List<double>();
            for (int i = 0; i < resultados.Count(); i++)
            {
                if (i == 0)
                {
                    flujos.Add(resultados[i].flujoEmisorEscudo.Value);
                }
                else
                {
                    flujos.Add(-resultados[i].flujoEmisorEscudo.Value);
                }

            }
            var arrFlujos = flujos.ToArray();
            TIR = Financial.IRR(ref arrFlujos);
            TCEA = Math.Round( Math.Pow(TIR + 1, bono.diasporanio / estructuracion.frecCupon) - 1,7);
            return TCEA;
        }

        public static double HallarTREABonista(Estructuracion estructuracion, List<Periodo> resultados, Bono bono)
        {
            double TIR;
            double TCEA;
            List<double> flujo = new List<double>();
            for (int i = 0; i < resultados.Count(); i++)
            {
                if (i == 0)
                {
                    flujo.Add(-resultados[i].flujoBonista);
                }
                else
                {
                    flujo.Add(resultados[i].flujoBonista);
                }

            }
            var arrFlujos = flujo.ToArray();
            TIR = Financial.IRR(ref arrFlujos);
            TCEA = Math.Round( Math.Pow(TIR + 1, bono.diasporanio / estructuracion.frecCupon) - 1,7);
            return TCEA;
        }

        public static Rentabilidad ResultadosRentabilidad(Estructuracion estructuracion,List<Periodo>resultados, Bono bono)
        {
            return new Rentabilidad
            {
                TCEAEmisor = Math.Round(HallarTCEAEmisor(estructuracion, resultados, bono),7),
                TCEAEmisorEscudo = Math.Round(HallarTCEAEmisorEscudo(estructuracion,resultados,bono),7),
                TREABonista = Math.Round(HallarTREABonista(estructuracion,resultados,bono),7)

            };

        }

        public static Utilidad ResultadosUtilidad(Estructuracion estructuracion, List<Periodo> periodos)
        {
            return new Utilidad
            {
                precioActual = Math.Round(HallarVAN(estructuracion, periodos),2),
                utilidad = Math.Round( HallarUtilidad(HallarVAN(estructuracion, periodos), periodos),2)

            };
        }



    }
}
