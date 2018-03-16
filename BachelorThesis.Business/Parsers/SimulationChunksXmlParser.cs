using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using BachelorThesis.Business.DataModels;
using BachelorThesis.Business.Simulation;

namespace BachelorThesis.Business.Parsers
{
    public class SimulationChunksXmlParser
    {
        public List<SimulationChunk> Parse(ProcessInstance process, XElement root)
        {
            var chunks = new List<SimulationChunk>();
            var chunksElement = root.Descendants(XmlParsersConfig.ElementChunks);

            var chunkElements = chunksElement.Elements(XmlParsersConfig.ElementChunk);
            foreach (var chunkElement in chunkElements)
            {
                var chunk = new SimulationChunk();

                var eventElements = chunkElement.Elements(XmlParsersConfig.ElementEvent);

                foreach (var eventElement in eventElements)
                {
                    var transactionEvent = ParseTransactionEvent(eventElement);
                    chunk.AddStep(transactionEvent);
                }

                chunks.Add(chunk);
            }

            return chunks;
        }

        private TransactionEvent ParseTransactionEvent(XElement eventElement)
        {
            var eventType = (TransactionEventType)int.Parse(eventElement.Attribute(XmlParsersConfig.AttributeType)?.Value);
            var transsactionId = int.Parse(eventElement.Attribute(XmlParsersConfig.AttributeTransactionId)?.Value);
            var raisedBy = int.Parse(eventElement.Attribute(XmlParsersConfig.AttributeRaisedById)?.Value);
            var created = DateTime.ParseExact(eventElement.Attribute(XmlParsersConfig.AttributeCreate)?.Value, XmlParsersConfig.DateTimeFormat, CultureInfo.InvariantCulture);

            switch (eventType)
            {
                case TransactionEventType.CompletionChanged:
                    var completionChangedElement = eventElement.Element(XmlParsersConfig.ElementCompletionChanged);
                    var completion = (TransactionCompletion)Enum.Parse(typeof(TransactionCompletion), completionChangedElement?.Attribute(XmlParsersConfig.AttributeCompletion)?.Value);

                    return new CompletionChangedTransactionEvent(transsactionId, raisedBy, created, completion);
                case TransactionEventType.InitiatorAssigned:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return null;
        }

        public XElement Create(List<SimulationChunk> chunks)
        {
            var root = new XElement(XmlParsersConfig.ElementChunks);


            foreach (var chunk in chunks)
            {
                var events = chunk.GetEvents();
                var chunkElement = new XElement(XmlParsersConfig.ElementChunk);
                foreach (var tEvent in events)
                {
                    var eventElement = new XElement(XmlParsersConfig.ElementEvent,
                        new XAttribute(XmlParsersConfig.AttributeType, (int)tEvent.EventType),
                        new XAttribute(XmlParsersConfig.AttributeTransactionId, tEvent.TransactionInstanceId),
                        new XAttribute(XmlParsersConfig.AttributeRaisedById, tEvent.RaisedByActorId),
                        new XAttribute(XmlParsersConfig.AttributeCreate, tEvent.Created.ToString(XmlParsersConfig.DateTimeFormat)));

                    if (tEvent.EventType == TransactionEventType.CompletionChanged)
                    {
                        eventElement.Add(new XElement(XmlParsersConfig.ElementCompletionChanged,
                            new XAttribute(XmlParsersConfig.AttributeCompletion, ((CompletionChangedTransactionEvent)tEvent).Completion)));
                    }

                    chunkElement.Add(eventElement);
                }

                root.Add(chunkElement);
            }

            return root;


        }
    }
}