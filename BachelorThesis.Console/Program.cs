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
    class Program
    {
        static void DisplayProcess(ProcessKind process)
        {
            Console.WriteLine(process.Name);

            foreach (var transaction in process.GetTransactions())
            {
                DisplayTransaction(process, transaction);
            }
        }

        static void DisplayTransaction(ProcessKind process, TransactionKind kind, int depth = 0)
        {
            for (int i = 0; i < depth*2; i++)
            {
                Console.Write(" ");
            }
            Console.WriteLine($"-{kind.Name}");
            foreach (var link in process.GetLinksAsSourceForTransaction(kind.Id))
            {
                DisplayLink(link);
            }
            foreach (var child in kind.GetChildren())
            {
                DisplayTransaction(process, child, depth + 1);
            }
        }

        static void DisplayLink(TransactionLink link)
        {
           // Console.WriteLine($" {link.SourceEventKindId} -> {link.DestinationEventKindId} ");
        }

        static void IterateChildren(TransactionInstance instance, DataTable table)
        {
            foreach (var child in instance.GetChildren())
            {
                table.Rows.Add(child.Id, child.Identificator, child.Completion, child.CompletionType,child.ParentId);
                IterateChildren(child,table);
            }
        }

        static DataTable CreateDataTable(List<TransactionInstance> transactions)
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
                table.Rows.Add(root.Id, root.Identificator, root.Completion, root.CompletionType,root.ParentId);

             //   IterateChildren(root,table);
                //TreeStructureHelper.IterateThrough<TransactionInstance,DataTable,object>(root,table, (node, t) =>
                //{
                //    t.Rows.Add(node.Id, node.Identificator, node.Completion, node.CompletionType, node.ParentId);
                //} );

                TreeStructureHelper.Traverse(root,table, (node, t) =>
                {
                    t.Rows.Add(node.Id, node.Identificator, node.Completion, node.CompletionType, node.ParentId);
                });
            }

            return table;

        }

        static void PrintTransaction(ProcessInstance process)
        {
            Console.WriteLine("-------------------------------------------------------------------------");
            Console.WriteLine("----------------------------TRANSACTIONS---------------------------------");

            var table = CreateDataTable(process.GetTransactions());
            ConsoleTableBuilder.From(table).ExportAndWriteLine();

            NextCmd(process);

        }

        static void NextCmd(ProcessInstance process)
        {
            Console.WriteLine("T: transaction view. <other> continue", Color.LawnGreen);
            var key = Console.ReadKey(true);
            if(key.Key == ConsoleKey.T)
                PrintTransaction(process);
        }

        

      

        [STAThread]
        static void Main(string[] args)
        {
            //var def = new RentalContractProcessDefinition();
            //var process = def.GetDefinition();

            //var doc = PrepareProcessDocument(process);
            //System.Console.WriteLine(doc.ToString());

            //doc.Save("TestCases/definition.xml");


            //  Console.WriteLine(DateTime.ParseExact("01-02-2018 15:34:23", XmlParsersConfig.DateTimeFormat, CultureInfo.InvariantCulture));

            var simulation = new RentalContractSimulationFromXml("SimulationCases/case-01.xml");

            simulation.Prepare();

            Console.WriteLine($"Simulation {simulation.Name} prepared");
            Console.ReadKey(true);
            
            PrintTransaction(simulation.ProcessInstance);

            while (simulation.CanContinue)
            {
                var results = simulation.SimulateNextChunk();

                foreach (var transactionEvent in results)
                {
                    var transaction = simulation.ProcessInstance.GetTransactionById(transactionEvent.TransactionInstanceId);

                    Console.WriteLine($"Event '{transactionEvent.EventType}' affected transaction #{transaction.Id} and occured at {transactionEvent.Created}. Raised by #{transactionEvent.RaisedByActorId}");
                    Console.WriteLine();
                }
                NextCmd(simulation.ProcessInstance);
            }

            //var eventDefinition = new RentalContractEventDefinitions();
            //var definition = new RentalContractProcessDefinition();
            //var processKind = definition.GetDefinition();

            //var rentalInstance = processKind.NewInstance(DateTime.Now);

            //var t1 = processKind.GetTransactionByIdentifier("T1").NewInstance(rentalInstance.Id);
            //var t2 = processKind.GetTransactionByIdentifier("T2").NewInstance(rentalInstance.Id);
            //var t3 = processKind.GetTransactionByIdentifier("T3").NewInstance(rentalInstance.Id);
            //var t4 = processKind.GetTransactionByIdentifier("T4").NewInstance(rentalInstance.Id);
            //var t5 = processKind.GetTransactionByIdentifier("T5").NewInstance(rentalInstance.Id);

            //rentalInstance.AddTransaction(t1);
            //rentalInstance.AddTransaction(t2);
            //rentalInstance.AddTransaction(t3);
            //rentalInstance.AddTransaction(t4);
            //rentalInstance.AddTransaction(t5);



            //var jsonModel = JsonConvert.SerializeObject(processKind, Formatting.Indented);
            //   var jsonInstance = JsonConvert.SerializeObject(rentalInstance, Formatting.Indented);

            ////  Console.WriteLine(jsonModel);
            // Console.WriteLine(jsonInstance);




            //Console.WriteLine("---- SIMULATION --- ");
            //var simulation = new RentalContractSimulation(rentalInstance);
            //simulation.Prepare();

            //while (simulation.CanContinue)
            //{
            //    var results = simulation.SimulateNextChunk();

            //    foreach (var step in results)
            //    {
            //        Console.WriteLine(JsonConvert.SerializeObject(step, Formatting.Indented));
            //        var eventInstance = step.Event;
            //        var eventName = eventDefinition.FindById(eventInstance.TransactionEventKindId).FirstName;

            //        Console.WriteLine($"For '{step.AffectedTransaction.Identificator} 'Event '{eventName}' occured {eventInstance.Created.ToShortDateString()} and raisedBy {eventInstance.RaisedByActorId}");
            //        if (eventInstance is CompletionChangedTransactionEvent @completionChangedEvent)
            //        {
            //            Console.WriteLine($"\t -- changed from {completionChangedEvent.OldCompletion} to {completionChangedEvent.NewCompletion}");
            //        }

            //    }

            //    Console.WriteLine();
            //    Console.WriteLine("...");
            //    Console.ReadKey();
            //    Console.WriteLine();
            //}


            //            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
            //<Simulation FirstName=""Case01"">
            //    <SimulationChunk>
            //        <SimulationStep>
            //            <EventInstance Type=""CompletionChanged"" TransactionId=""1"" RaisedById=""1"" Created=""Date"">
            //                <CompletionChanged OldCompletion=""Request"" NewCompletion=""Promise"" />
            //            </EventInstance>
            //        </SimulationStep>        
            //    </SimulationChunk>
            //</Simulation>";

            //            var xdoc = XDocument.Parse(xml);
            //            var chunks = xdoc.Descendants("SimulationChunk");
            //            foreach (var chunk in chunks)
            //            {
            //                var steps = chunk.Elements("SimulationStep");
            //                foreach (var step in steps)
            //                {
            //                    var ev = step.Element("EventInstance");
            //                    var evType = ev.Attribute("Type").Value;

            //                    if (evType == "CompletionChanged")
            //                    {
            //                        var evArgs = ev.Element("CompletionChanged");
            //                        Console.WriteLine(evArgs.Attribute("OldCompletion").Value);
            //                        Console.WriteLine(evArgs.Attribute("NewCompletion").Value);
            //                    }
            //                    Console.WriteLine(evType);
            //                }
            //            }

            Console.WriteLine("---- END ----");
            Console.ReadKey();
        }
    }
};

