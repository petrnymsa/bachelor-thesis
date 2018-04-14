using System.Collections.Generic;
using BachelorThesis.Business.DataModels;

namespace BachelorThesis.Business.Parsers
{
    public class SimulationModelDefinition
    {
        public ProcessKind ProcessKind { get; set; }
        public List<ActorRole> ActorRoles { get; set; }
    }
}