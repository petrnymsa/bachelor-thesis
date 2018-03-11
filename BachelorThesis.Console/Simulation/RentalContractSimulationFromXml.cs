using System.Xml.Linq;
using BachelorThesis.Bussiness.DataModels;

namespace BachelorThesis.ConsoleTest
{
    public class RentalContractSimulationFromXml : ProcessSimulation
    {
        private string xmlPath;

        private TransactionInstance rentalContracting;
        private TransactionInstance rentalPayment;
        private TransactionInstance carPickUp;
        private TransactionInstance carDropOff;
        private TransactionInstance penaltyPayment;

        public RentalContractSimulationFromXml(string xmlPath)
        {
            this.xmlPath = xmlPath;
        }


        public override void Prepare()
        {
            var xdoc = XDocument.Load(xmlPath);
            var processInstanceParser = new ProcessInstanceXmlParser();
            var simulationChunksParser = new SimulationChunksXmlParser();

            Name = xdoc.Root?.Attribute("FirstName")?.Value;

            ProcessInstance = processInstanceParser.Parse(xdoc.Root);

            var chunks = simulationChunksParser.Parse(ProcessInstance, xdoc.Root);
            foreach (var chunk in chunks)
                AddChunk(chunk);
        }
    }
}