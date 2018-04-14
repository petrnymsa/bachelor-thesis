using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BachelorThesis.Business;
using BachelorThesis.Business.DataModels;
using BachelorThesis.Business.Parsers;
using BachelorThesis.Business.Simulation;

namespace BachelorThesis.Data
{
    public class TransactionCActByTime
    {
        public DateTime Creation { get; set; }
        public TransactionCompletion CAct { get; set; }
        public int TransactionKindId { get; set; }
        public string TransactionLabel { get; set; }


        public TransactionCActByTime(DateTime creation, TransactionCompletion cAct, int transactionKindId, string transactionLabel)
        {
            Creation = creation;
            CAct = cAct;
            TransactionKindId = transactionKindId;
            TransactionLabel = transactionLabel;
        }
    }

    public class RandomDataFaker
    {
        private Random rnd;

        public RandomDataFaker()
        {
            rnd= new Random();
        }
        //public ProcessInstance CreateProcessInstance(ProcessKind kind)
        //{
        //    var completion = GetRandomCompletion();
        //    var startDate = GetRandomDate();
        //    var endDate = startDate.AddHours(rnd.Next(1, 24));

        //    var instance = new ProcessInstance() { }
              

        //}

        public TransactionCompletion GetRandomCompletion()
        {
            return (TransactionCompletion)rnd.Next(1, 9);
        }

        public DateTime GetRandomDate()
        {
            return DateTime.Now.AddDays(rnd.Next(1, 260));
        }
    }

    public class FakeDataStorage
    {
        public List<ProcessInstance> ProcessInstances { get; set; }
        public List<TransactionEvent> TransactionEvents { get; set; }   

        
        
    }

    public class DataAggregator
    {
        //public List<TransactionCActByTime> GetDailyRentalContractRequests()
        //{
            
        //}
    }

    public class SimulationLoader
    {

        private async Task<ProcessKind> LoadModelDefinition()
        {
             var xml = await SimulationCases.LoadXmlAsync(SimulationCases.ModelDefinition);
            var parser = new ProcessKindXmlParser();
            var kind = parser.ParseDefinition(xml);

            return kind;
        }

        public async Task<ProcessSimulation> Load(string caseName)
        {
            var xml = await SimulationCases.LoadXmlAsync(caseName);
            var simulation = new RentalContractSimulationFromXml(xml);

            simulation.ProcessKind = await LoadModelDefinition();

            return simulation;
        }
    }
}
