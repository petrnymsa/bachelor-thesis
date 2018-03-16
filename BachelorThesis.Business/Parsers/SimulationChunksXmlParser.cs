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
        private const string ElementChunks = "Chunks";
        private const string ElementChunk = "Chunk";
        private const string ElementEvent = "Event";
        private const string AttributeType = "Type";
        private const string AttributeTransactionId = "TransactionId";
        private const string AttributeRaisedById = "RaisedById";
        private const string AttributeCreate = "Created";
        private const string ElementCompletionChanged = "CompletionChanged";
        private const string AttributeCompletion = "Completion";

        public List<SimulationChunk> Parse(ProcessInstance process, XElement root)
        {
            var chunks = new List<SimulationChunk>();
            var chunksElement = root.Descendants(ElementChunks);

            var chunkElements = chunksElement.Elements(ElementChunk);
            foreach (var chunkElement in chunkElements)
            {
                var chunk = new SimulationChunk();

                var eventElements = chunkElement.Elements(ElementEvent);

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
            var eventType = (TransactionEventType)int.Parse(eventElement.Attribute(AttributeType)?.Value);
            var transsactionId = int.Parse(eventElement.Attribute(AttributeTransactionId)?.Value);
            var raisedBy = int.Parse(eventElement.Attribute(AttributeRaisedById)?.Value);
            var created = DateTime.ParseExact(eventElement.Attribute(AttributeCreate)?.Value, XmlParsersConfig.DateTimeFormat, CultureInfo.InvariantCulture);

            switch (eventType)
            {
                case TransactionEventType.CompletionChanged:
                    var completionChangedElement = eventElement.Element(ElementCompletionChanged);

                    //    var oldCompletion = (TransactionCompletion)Enum.Parse(typeof(TransactionCompletion), completionChangedElement?.Attribute("OldCompletion")?.Value);
                    // var newCompletion = (TransactionCompletion)Enum.Parse(typeof(TransactionCompletion), completionChangedElement?.Attribute("NewCompletion")?.Value);
                    var completion = (TransactionCompletion)Enum.Parse(typeof(TransactionCompletion), completionChangedElement?.Attribute(AttributeCompletion)?.Value);


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
            var root = new XElement(ElementChunks);


            foreach (var chunk in chunks)
            {
                var events = chunk.GetEvents();
                var chunkElement = new XElement(ElementChunk);
                foreach (var tEvent in events)
                {
                    var eventElement = new XElement(ElementEvent,
                        new XAttribute(AttributeType, (int)tEvent.EventType),
                        new XAttribute(AttributeTransactionId, tEvent.TransactionInstanceId),
                        new XAttribute(AttributeRaisedById, tEvent.RaisedByActorId),
                        new XAttribute(AttributeCreate, tEvent.Created.ToString(XmlParsersConfig.DateTimeFormat)));

                    if (tEvent.EventType == TransactionEventType.CompletionChanged)
                    {
                        eventElement.Add(new XElement(ElementCompletionChanged,
                            new XAttribute(AttributeCompletion, ((CompletionChangedTransactionEvent)tEvent).Completion)));
                    }

                    chunkElement.Add(eventElement);
                }

                root.Add(chunkElement);
            }

            return root;


        }
    }
}