﻿using System;
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
            return new Estructuracion
            {
                totalPeriodos = (bono.diasporanio / bono.frecuencia) * bono.nroaños,

                TEA = bono.tipoTasa == "Efectiva" ? bono.tasaInteres : HallarTEA(bono.tasaInteres, bono.diasporanio, bono.capitalizacion),
                TEP = Math.Round(tea * 100,9),
                COK = Math.Round(Math.Pow( bono.tasaDescuentoCOK, (double)bono.frecuencia / bono.diasporanio), 7),
                frecCupon = bono.frecuencia,
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
                plazoGracia = null,
                bono = 0,
                cupon = 0,
                amortizacion = 0,
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
                aux.plazoGracia = null;
                aux.bono = i == 0 ? bono.vnominal : Math.Round(lista[i].bono.Value - lista[i].amortizacion.Value, 2);
                aux.cupon = Math.Round(aux.bono.Value * (estructuracion.TEP)/100, 2);
                aux.amortizacion = aux.N == estructuracion.totalPeriodos ? aux.bono.Value :0.00 ;
                aux.cuota = aux.cupon.Value + aux.amortizacion.Value;
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
            double amortizacionC = Math.Round(bono.vnominal / estructuracion.totalPeriodos,2);
            for (int i = 0; i < estructuracion.totalPeriodos; i++)
            {
                Periodo aux = new Periodo();
                aux.N = i + 1;
                aux.plazoGracia = null;
                aux.bono = i == 0 ? bono.vnominal : Math.Round(lista[i].bono.Value - lista[i].amortizacion.Value, 2);
                aux.cupon = Math.Round(aux.bono.Value * (estructuracion.TEP) / 100, 2);
                aux.amortizacion = amortizacionC;
                aux.cuota = Math.Round(aux.cupon.Value + aux.amortizacion.Value,2);
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
            VAN = Financial.NPV(estructuracion.COK/100,  ref arrFlujos);
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
            TCEA = Math.Pow(TIR + 1, bono.diasporanio / estructuracion.frecCupon) - 1;
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
            TCEA = Math.Pow(TIR + 1, bono.diasporanio / estructuracion.frecCupon) - 1;
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
