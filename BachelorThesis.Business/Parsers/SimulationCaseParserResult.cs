using System.Collections.Generic;
using BachelorThesis.Business.DataModels;
using BachelorThesis.Business.Simulation;

namespace BachelorThesis.Business.Parsers
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