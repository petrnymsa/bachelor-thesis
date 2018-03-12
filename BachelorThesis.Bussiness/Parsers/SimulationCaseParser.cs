using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
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

    public class SimulationCaseParser
    {
        private const string ElementActor = "Actor";
        private const string ElementActors = "Actors";
        private const string AttributeId = "Id";
        private const string AttributeActorKindId = "ActorKindId";
        private const string AttributeFirstName = "FirstName";
        private const string AttributeLastName = "LastName";

        public SimulationCaseParserResult Parse(string xmlPath)
        {
            var result = new SimulationCaseParserResult();
            var doc = XDocument.Load(xmlPath);
            var processParser = new ProcessInstanceXmlParser();
            var chunksParser = new SimulationChunksXmlParser();

            var instance = processParser.Parse(doc.Root);
            var chunks = chunksParser.Parse(instance, doc.Root);

            result.Name = doc.Root.Attribute("Name").Value;

            var actorsElement = doc.Root.Element(ElementActors);

            var actors = ParseActorElements(actorsElement);


            result.Actors = actors;
            result.ProcessInstance = instance;
            result.Chunks = chunks;

            return result;
        }

        public XDocument Create(SimulationCaseParserResult data)
        {
            var root = new XElement("Simulation", new XAttribute("Name", data.Name));

            var actorsElement = new XElement("Actors");

            foreach (var actor in data.Actors)
            {
                actorsElement.Add(CreateActorElement(actor));
            }

            root.Add(actorsElement);

            var transactionsElement = new ProcessInstanceXmlParser().Create(data.ProcessInstance);

            root.Add(transactionsElement);

            var chunksElement = new SimulationChunksXmlParser().Create(data.Chunks);

            root.Add(chunksElement);


            return new XDocument(root);
        }

        private XElement CreateActorElement(Actor actor)
        {
            return new XElement("Actor",
                new XAttribute(AttributeId, actor.Id),
                new XAttribute(AttributeActorKindId, actor.ActorKindId),
                new XAttribute(AttributeFirstName, actor.FirstName),
                new XAttribute(AttributeLastName, actor.LastName));
        }

        private static List<Actor> ParseActorElements(XElement actorsElement)
        {
            var actorElements = actorsElement.Elements(ElementActor);

            return (from actorElement in actorElements
                let id = int.Parse(actorElement.Attribute(AttributeId).Value)
                let kindId = int.Parse(actorElement.Attribute(AttributeActorKindId).Value)
                let firstName = actorElement.Attribute(AttributeFirstName).Value
                let lastName = actorElement.Attribute(AttributeLastName).Value
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
