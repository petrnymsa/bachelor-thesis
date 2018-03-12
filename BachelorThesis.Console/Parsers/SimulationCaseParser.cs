using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BachelorThesis.Bussiness.DataModels;

namespace BachelorThesis.ConsoleTest.Parsers
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

    public class SimulationCaseParser
    {
        public SimulationCaseParserResult Parse(string xmlPath)
        {
            var result = new SimulationCaseParserResult();
            var doc = XDocument.Load(xmlPath);
            var processParser = new ProcessInstanceXmlParser();
            var chunksParser = new SimulationChunksXmlParser();

            var instance = processParser.Parse(doc.Root);
            var chunks = chunksParser.Parse(instance, doc.Root);

            result.Name = doc.Root.Attribute("Name").Value;

            var actorsElement = doc.Root.Element("Actors");

            var actors = ParseActorElements(actorsElement);


            result.Actors = actors;
            result.ProcessInstance = instance;
            result.Chunks = chunks;

            return result;
        }

        private static List<Actor> ParseActorElements(XElement actorsElement)
        {
            var actorElements = actorsElement.Elements("Actor");

            return (from actorElement in actorElements
                let id = int.Parse(actorElement.Attribute("Id").Value)
                let kindId = int.Parse(actorElement.Attribute("ActorKindId").Value)
                let firstName = actorElement.Attribute("FirstName").Value
                let lastName = actorElement.Attribute("LastName").Value
                select new Actor()
                {
                    Id = id,
                    ActorKindId = kindId,
                    FirstName = firstName,
                    LastName = lastName
                }).ToList();
        }
    }
}
