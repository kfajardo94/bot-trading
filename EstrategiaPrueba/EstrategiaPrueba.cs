// Copyright QUANTOWER LLC. © 2017-2021. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using TradingPlatform.BusinessLayer;

namespace EstrategiaPrueba
{



    /// <summary>
    /// An example of strategy for working with one symbol. Add your code, compile it and run via Strategy Runner panel in the assigned trading terminal.
    /// Information about API you can find here: http://api.quantower.com
    /// </summary>
	public class EstrategiaPrueba : Strategy
    {
        [InputParameter("Symbol", 10)]
        private Symbol symbol;

        [InputParameter("Account", 20)]
        public Account account;

        private MediasMovilesFajardo.SMA20 sm20 = new MediasMovilesFajardo.SMA20();

        private Indicator RSI = Core.Indicators.BuiltIn.RSI(14, PriceType.Open, RSIMode.Exponential, MaMode.LWMA, 1);
            
        double precio = 11070;

        public override string[] MonitoringConnectionsIds => new string[] { this.symbol?.ConnectionId };

        public EstrategiaPrueba()
            : base()
        {
            // Defines strategy's name and description.
            this.Name = "EstrategiaPrueba";
            this.Description = "My strategy's annotation";
        }

        /// <summary>
        /// This function will be called after creating a strategy
        /// </summary>
        protected override void OnCreated()
        {
            // Add your code here
            
        }

        /// <summary>
        /// This function will be called after running a strategy
        /// </summary>
        protected override void OnRun()
         
        {

            if (symbol == null || account == null || symbol.ConnectionId != account.ConnectionId)
            {
                Log("Incorrect input parameters... Symbol or Account are not specified or they have diffent connectionID.", StrategyLoggingLevel.Error);
                OnStop();
                return;
            }

            this.symbol = Core.GetSymbol(this.symbol?.CreateInfo());

            if (this.symbol != null)
            {
                this.symbol.NewQuote += SymbolOnNewQuote;
                this.symbol.NewLast += SymbolOnNewLast;
            }


            DateTime fechaActual = DateTime.UtcNow;
            int valor = ((int) fechaActual.DayOfWeek)-2;
            LogInfo("valor1: "+valor);
            fechaActual = new DateTime(
            fechaActual.Year,
            fechaActual.Month,
            fechaActual.Day-valor,
            fechaActual.Hour,
            55,
            0,
            0);


            DateTime fechaInicioSemana = DateTime.UtcNow;
            valor = ((int)fechaInicioSemana.DayOfWeek)-2;
            LogInfo("valor2: "+valor);
            fechaInicioSemana = new DateTime(
                fechaInicioSemana.Year,
                fechaInicioSemana.Month,
                fechaInicioSemana.Day-valor,
                fechaInicioSemana.Hour,
                0,
                0,
                0);

            LogInfo("fecha inicio: "+ fechaInicioSemana);
            LogInfo("fecha fin: "+fechaActual);
            int horas1 = fechaActual.Minute;
            int horas2 = fechaInicioSemana.Minute;

            LogInfo("1: "+horas1);
            LogInfo("2: "+horas2);
            LogInfo("3: "+(horas1 - horas2));

            getHistoricoBarras(fechaInicioSemana, fechaActual);

            // Add your code here


        }

        /// <summary>
        /// This function will be called after stopping a strategy
        /// </summary>
        protected override void OnStop()
        {
            Log("detener fajardo: ", StrategyLoggingLevel.Info);
            if (this.symbol != null)
            {
                this.symbol.NewQuote -= SymbolOnNewQuote;
                this.symbol.NewLast -= SymbolOnNewLast;
            }

            // Add your code here
        }

        /// <summary>
        /// This function will be called after removing a strategy
        /// </summary>
        protected override void OnRemove()
        {
            this.symbol = null;
            this.account = null;
            // Add your code here
        }

        /// <summary>
        /// Use this method to provide run time information about your strategy. You will see it in StrategyRunner panel in trading terminal
        /// </summary>
        protected override List<StrategyMetric> OnGetMetrics()
        {
            List<StrategyMetric> result = base.OnGetMetrics();

            // An example of adding custom strategy metrics:
            // result.Add("Opened buy orders", "2");
            // result.Add("Opened sell orders", "7");
            

            return result;
        }

        private void SymbolOnNewQuote(Symbol symbol, Quote quote)
        {
            // Add your code here
            //LogInfo("Nueva Quote: "+quote);
        }

        private void SymbolOnNewLast(Symbol symbol, Last last)
        {
            // Add your code here
            if (last.Price > precio)
            {
                //LogInfo("If: " + last.Price + " - " + precio);
                //Core.Instance.PlaceOrder(this.symbol, this.account, Side.Sell, TimeInForce.Day);
            } else
            {
                //LogInfo("Else: " + last.Price + " - " + precio);
                //Core.Instance.PlaceOrder(this.symbol, this.account, Side.Buy, TimeInForce.Day);
            }
        }

        

        public void getHistoricoBarras(DateTime fechaInicio, DateTime fechaActual)
        {

            
            HistoricalData historicalData = this.symbol.GetHistory(Period.MIN5, fechaInicio, fechaActual);
            //LogInfo("info: "+historicalData.Count);
            for (int i = 0; i < historicalData.Count; i++)
            {
                HistoryItemBar hIB = (HistoryItemBar) historicalData[i];
                if (i < 100)
                {
                //    LogInfo("i: "+ i + " - " +hIB);
                }

                if (i >= 100)
                {
                  //  LogInfo("i2: "+ i + " - " +hIB);
                }

            }


        }

        private void LogInfo(Object valor)
        {
            Log(valor.ToString(), StrategyLoggingLevel.Info);
        }

    }
}
