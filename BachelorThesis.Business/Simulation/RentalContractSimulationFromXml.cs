using BachelorThesis.Business.DataModels;
using BachelorThesis.Business.Parsers;

namespace BachelorThesis.Business.Simulation
{
    public class RentalContractSimulationFromXml : ProcessSimulation
    {
        private readonly string xml;

        public RentalContractSimulationFromXml(string xml)
        {
            this.xml = xml;
        }


        public override void Prepare()
        {
            var parser = new SimulationCaseParser();
            var result = parser.Parse(xml);

            ProcessInstance = result.ProcessInstance;
            Name = result.Name;

            foreach (var actor in result.Actors)
                Actors.Add(actor);

            foreach (var chunk in result.Chunks)
                AddChunk(chunk);

            
        }

        public void Reset()
        {
            base.ResetChunks();
            Actors.Clear();
            Prepare();
            
        }
    }
}