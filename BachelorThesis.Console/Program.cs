﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using BachelorThesis.Business;
using BachelorThesis.Business.DataModels;
using BachelorThesis.Business.Simulation;
using Colorful;
using ConsoleTableExt;
using Console = Colorful.Console;

namespace BachelorThesis.ConsoleTest
{
    public static class SimulatorPrinter
    {
        private static void PrintLineWithMarker(string marker, string line, Color markerColor)
        {
            StyleSheet styleSheet = new StyleSheet(Color.MintCream);
            styleSheet.AddStyle(@"\[.*\]", markerColor);

            Console.WriteLineStyled($"[{marker}] {line}", styleSheet);
        }

        public static void PrintInfo(string line) => PrintLineWithMarker("INFO", line, Color.Aquamarine);
        public static void PrintWarning(string line) => PrintLineWithMarker("WARNING", line, Color.Yellow);
        public static void PrintError(string line) => PrintLineWithMarker("ERROR", line, Color.DarkRed);
        public static void PrintEvent(string line) => PrintLineWithMarker("EVENT", line, Color.DarkSalmon);
    }


    internal class Simulator
    {
        private readonly ProcessSimulation simulation;

        public Simulator(ProcessSimulation simulation)
        {
            this.simulation = simulation;
        }

        public void Start()
        {
            simulation.Prepare();

            Console.WriteLine($"Simulation {simulation.Name} prepared");

            PrintTransactions(simulation.ProcessInstance);

            while (simulation.CanContinue)
            {
                var results = simulation.SimulateNextChunk();

                foreach (var transactionEvent in results)
                {
                    var transaction = simulation.ProcessInstance.GetTransactionById(transactionEvent.TransactionInstanceId);
                    var actor = simulation.FindActorById(transactionEvent.RaisedByActorId);
                    //    Console.WriteLine($"[{transactionEvent.Created}] Event '{transactionEvent.EventType}' affected transaction '{transaction.Identificator}'. Raised by '{actor.FullName}'");
                    Console.WriteLineFormatted("[{0}] Event affected transaction '{1}'. Raised by '{2}'", Color.Moccasin, Color.WhiteSmoke, new[]
                    {
                        transactionEvent.Created.ToString(),
                        transaction.Identificator,
                        actor.FullName
                    });


                    Console.Write($"\tTransaction's state changed to ");
                    Console.WriteLine(transactionEvent.Completion, Color.Salmon);
                    Console.WriteLine();
                }
                NextCmd(simulation.ProcessInstance);
            }
        }

        private static DataTable CreateDataTable(List<TransactionInstance> transactions)
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Identificator", typeof(string));
            table.Columns.Add("Completion", typeof(float));
            table.Columns.Add("Completion type", typeof(String));
            table.Columns.Add("Parent id", typeof(int));

            //     table.Columns.Add("", typeof(DateTime));

            foreach (var root in transactions)
            {
                table.Rows.Add(root.Id, root.Identificator, root.CompletionNumber, root.Completion, root.ParentId);
                TreeStructureHelper.Traverse(root, table, (node, t) => t.Rows.Add(node.Id, node.Identificator, node.CompletionNumber, node.Completion, node.ParentId));
            }

            return table;

        }

        private static void PrintTransactions(ProcessInstance process)
        {
            Console.WriteLine("-------------------------------------------------------------------------");
            Console.WriteLine("----------------------------TRANSACTIONS---------------------------------");

            var table = CreateDataTable(process.GetTransactions());
            ConsoleTableBuilder.From(table).ExportAndWriteLine();

            NextCmd(process);

        }

        private static void NextCmd(ProcessInstance process)
        {
            SimulatorPrinter.PrintInfo("T: transaction view. <other> continue");
            //Console.Write("T: transaction view. <other> continue", Color.LawnGreen);
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.T)
                PrintTransactions(process);
            else Console.WriteLine("\n -#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#- \n", Color.MediumSeaGreen);
        }



    }

    internal class Program
    {
      

        [STAThread]
        private static void Main(string[] args)
        {
            var xml = SimulationCases.LoadXmlAsync(SimulationCases.Case01).Result;
            var simulation = new RentalContractSimulationFromXml(xml);
            var simulator = new Simulator(simulation);

            SimulatorPrinter.PrintInfo("blablabla balbla");

            simulator.Start();

            Console.WriteLine("---- END ----");
            Console.ReadKey();
        }
    }
};

