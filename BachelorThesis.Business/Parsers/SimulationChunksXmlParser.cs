using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Linq;
using BachelorThesis.Business.DataModels;
using BachelorThesis.Business.Simulation;

namespace BachelorThesis.Business.Parsers
{
    public class SimulationChunksXmlParser
    {
        public List<SimulationChunk> Parse(XElement root)
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

        private TransactionEvent ParseTransactionEvent(XElement element)
        {
            //var eventType = (TransactionEventType)int.Parse(element.Attribute(XmlParsersConfig.AttributeType)?.Value);
            //var transsactionId = int.Parse(element.Attribute(XmlParsersConfig.AttributeTransactionId)?.Value);
            //var transactionKindId = int.Parse(element.Attribute(XmlParsersConfig.AttributeTransactionKindId)?.Value);
            //var raisedBy = int.Parse(element.Attribute(XmlParsersConfig.AttributeRaisedById)?.Value);
            var eventType = (TransactionEventType)ParseIntAttribute(element, XmlParsersConfig.AttributeType);
            var transactionId = ParseIntAttribute(element, XmlParsersConfig.AttributeTransactionId);
            var transactionKindId = ParseIntAttribute(element, XmlParsersConfig.AttributeTransactionKindId);
            var raisedBy = ParseIntAttribute(element, XmlParsersConfig.AttributeRaisedById);

            var created = DateTime.ParseExact(element.Attribute(XmlParsersConfig.AttributeCreate)?.Value, XmlParsersConfig.DateTimeFormat, CultureInfo.InvariantCulture);

            var completionChangedElement = element.Element(XmlParsersConfig.ElementCompletionChanged);
            var completion = (TransactionCompletion)Enum.Parse(typeof(TransactionCompletion), completionChangedElement?.Attribute(XmlParsersConfig.AttributeCompletion)?.Value);


            return new TransactionEvent(eventType, transactionId, transactionKindId, raisedBy, created, completion);
//            switch (eventType)
//            {
//                case TransactionEventType.CompletionChanged:
//                    var completionChangedElement = element.Element(XmlParsersConfig.ElementCompletionChanged);
//                    var completion = (TransactionCompletion)Enum.Parse(typeof(TransactionCompletion), completionChangedElement?.Attribute(XmlParsersConfig.AttributeCompletion)?.Value);
//
//                    return new CompletionChangedTransactionEvent(transactionId, transactionKindId, raisedBy, created, completion);
//                case TransactionEventType.InitiatorAssigned:
//                    break;
//                default:
//                    throw new ArgumentOutOfRangeException();
//            }
//
//            return null;
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
                        new XAttribute(XmlParsersConfig.AttributeTransactionKindId, tEvent.TransactionKindId),
                        new XAttribute(XmlParsersConfig.AttributeRaisedById, tEvent.RaisedByActorId),
                        new XAttribute(XmlParsersConfig.AttributeCreate, tEvent.Created.ToString(XmlParsersConfig.DateTimeFormat)));

                    eventElement.Add(new XElement(XmlParsersConfig.ElementCompletionChanged,
                        new XAttribute(XmlParsersConfig.AttributeCompletion, tEvent.Completion)));

//                    if (tEvent.EventType == TransactionEventType.CompletionChanged)
//                    {
//                        eventElement.Add(new XElement(XmlParsersConfig.ElementCompletionChanged,
//                            new XAttribute(XmlParsersConfig.AttributeCompletion, ((CompletionChangedTransactionEvent)tEvent).Completion)));
//                    }

                    chunkElement.Add(eventElement);
                }

                root.Add(chunkElement);
            }

            return root;


        }

        private static int ParseIntAttribute(XElement element, string attributeName)
        {
            var attribute = element.Attribute(attributeName);

            if(attribute == null)
                throw new ArgumentException($"Attribute {attributeName} does not exists or mispelled");

            return int.Parse(attribute.Value);
        }
    }
}