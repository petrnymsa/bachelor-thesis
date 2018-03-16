using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using BachelorThesis.Business.DataModels;

namespace BachelorThesis.Business.Parsers
{
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

            result.Name = doc.Root.Attribute(XmlParsersConfig.AttributeName).Value;

            var actorsElement = doc.Root.Element(XmlParsersConfig.ElementActors);

            var actors = ParseActorElements(actorsElement);


            result.Actors = actors;
            result.ProcessInstance = instance;
            result.Chunks = chunks;

            return result;
        }

        public XDocument Create(SimulationCaseParserResult data)
        {
            var root = new XElement(XmlParsersConfig.ElementSimulation, new XAttribute(XmlParsersConfig.AttributeName, data.Name));

            var actorsElement = new XElement(XmlParsersConfig.ElementActors);

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
            return new XElement(XmlParsersConfig.ElementActor,
                new XAttribute(XmlParsersConfig.AttributeId, actor.Id),
                new XAttribute(XmlParsersConfig.AttributeActorRoleId, actor.ActorKindId),
                new XAttribute(XmlParsersConfig.AttributeFirstName, actor.FirstName),
                new XAttribute(XmlParsersConfig.AttributeLastName, actor.LastName));
        }

        private static List<Actor> ParseActorElements(XElement actorsElement)
        {
            var actorElements = actorsElement.Elements(XmlParsersConfig.ElementActor);

            return (from actorElement in actorElements
                let id = int.Parse(actorElement.Attribute(XmlParsersConfig.AttributeId).Value)
                let kindId = int.Parse(actorElement.Attribute(XmlParsersConfig.AttributeActorRoleId).Value)
                let firstName = actorElement.Attribute(XmlParsersConfig.AttributeFirstName).Value
                let lastName = actorElement.Attribute(XmlParsersConfig.AttributeLastName).Value
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
