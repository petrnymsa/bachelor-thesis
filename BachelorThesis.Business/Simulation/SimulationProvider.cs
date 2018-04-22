using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BachelorThesis.Business.DataModels;
using BachelorThesis.Business.Parsers;

namespace BachelorThesis.Business.Simulation
{
    public class SimulationProvider
    {
        public async Task<ProcessSimulation> LoadAsync(string caseName)
        {
            var processKind = await LoadDefinitionAsync();
            var caseXml = await SimulationCases.LoadXmlAsync(caseName);

            var simulation = new RentalContractSimulationFromXml(caseXml);
            simulation.Prepare();
            var transactions = simulation.ProcessInstance.GetTransactions();

            foreach (var instance in transactions)
            {
                TreeStructureHelper.Traverse<TransactionInstance,TransactionKind>(instance,
                    (node) => processKind.GetTransactionById(node.Id), (transactionInstance, transactionKind) =>
                {
                    transactionInstance.TransactionKind = transactionKind;
                });
              
            }

            return simulation;

        }

        private static async Task<ProcessKind> LoadDefinitionAsync()
        {
            var xml = await SimulationCases.LoadXmlAsync(SimulationCases.ModelDefinition);
            return new ProcessKindXmlParser().ParseDefinition(xml);
        }
        
    }
}
