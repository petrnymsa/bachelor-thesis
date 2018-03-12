using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using BachelorThesis.Bussiness.DataModels;
using Colorful;
using ConsoleTableExt;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;
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
                    //    Console.WriteLine($"[{transactionEvent.Created}] Event '{transactionEvent.EventType}' affected transaction '{transaction.Identificator}'. Raised by '{actor.Fullname}'");
                    Console.WriteLineFormatted("[{0}] Event '{1}' affected transaction '{2}'. Raised by '{3}'", Color.Moccasin, Color.WhiteSmoke, new[]
                    {
                        transactionEvent.Created.ToString(),
                        transactionEvent.EventType.ToString(),
                        transaction.Identificator,
                        actor.Fullname
                    });

                    switch (transactionEvent.EventType)
                    {
                        case TransactionEventType.CompletionChanged:
                            var cEvent = (CompletionChangedTransactionEvent)transactionEvent;
                            Console.Write($"\tTransaction's state changed to ");
                            Console.WriteLine(cEvent.NewCompletion, Color.Salmon);
                            break;
                        case TransactionEventType.InitiatorAssigned:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

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
                table.Rows.Add(root.Id, root.Identificator, root.Completion, root.CompletionType, root.ParentId);
                TreeStructureHelper.Traverse(root, table, (node, t) => t.Rows.Add(node.Id, node.Identificator, node.Completion, node.CompletionType, node.ParentId));
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
        }



    }

    internal class Program
    {
      

        [STAThread]
        private static void Main(string[] args)
        {
            var simulation = new RentalContractSimulationFromXml("SimulationCases/case-01.xml");
            var simulator = new Simulator(simulation);

            SimulatorPrinter.PrintInfo("blablabla balbla");

            simulator.Start();

            Console.WriteLine("---- END ----");
            Console.ReadKey();
        }
    }
};

