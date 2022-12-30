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

            
            DateTime fechaActual = DateTime.Now;
            //fechaActual = new DateTime(
            //fechaActual.Year,
            //fechaActual.Month,
            //fechaActual.Day,
            //fechaActual.Hour-5,
            //fechaActual.Minute,
            //fechaActual.Second,
            //fechaActual.Millisecond,
            //fechaActual.Kind);


            DateTime fechaInicioSemana = DateTime.Now;
            //int valor = ((int)fechaInicioSemana.DayOfWeek);
            fechaInicioSemana = new DateTime(
                fechaInicioSemana.Year,
                fechaInicioSemana.Month,
                fechaInicioSemana.Day-1,
                17,
                0,
                0,
                0,
                fechaInicioSemana.Kind);


            getHistoric(fechaInicioSemana, fechaActual);

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

        private void LogInfo(Object valor)
        {
            Log(valor.ToString(), StrategyLoggingLevel.Info);
        }

        public void getHistoric(DateTime fechaInicio, DateTime fechaActual)
        {

            int count = 0;
            HistoricalData historicalData = this.symbol.GetHistory(Period.MIN1, fechaInicio, fechaActual);
            for (int i = 0; i < historicalData.Count; i++)
            {
                HistoryItemBar hIB = (HistoryItemBar) historicalData[i];
                if (i < 100)
                {
                    //LogInfo("i: "+ i + " - " +hIB);
                }

            }


        }

    }
}
