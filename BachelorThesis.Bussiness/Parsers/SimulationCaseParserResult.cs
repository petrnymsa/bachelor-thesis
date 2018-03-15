using System.Collections.Generic;
using BachelorThesis.Bussiness.DataModels;
using BachelorThesis.Bussiness.Simulation;

namespace BachelorThesis.Bussiness.Parsers
{
    public class SimulationCaseParserResult
    {
        public string Name { get; set; }
        public ProcessInstance ProcessInstance { get; set; }
        public List<SimulationChunk> Chunks { get; set; }
        public List<Actor> Actors { get; set; }

        public SimulationCaseParserResult()
        {
            Chunks = new List<SimulationChunk>();
            Actors = new List<Actor>();
        }
    }
}