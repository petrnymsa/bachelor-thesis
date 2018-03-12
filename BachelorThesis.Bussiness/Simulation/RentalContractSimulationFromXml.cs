using BachelorThesis.Bussiness.DataModels;
using BachelorThesis.Bussiness.Parsers;

namespace BachelorThesis.Bussiness.Simulation
{
    public class RentalContractSimulationFromXml : ProcessSimulation
    {
        private readonly string xmlPath;

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
            var parser = new SimulationCaseParser();
            var result = parser.Parse(xmlPath);

            ProcessInstance = result.ProcessInstance;
            Name = result.Name;

            foreach (var actor in result.Actors)
                Actors.Add(actor);

            foreach (var chunk in result.Chunks)
                AddChunk(chunk);
        }

       
    }
}