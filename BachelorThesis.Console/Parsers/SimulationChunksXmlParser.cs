using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using BachelorThesis.Bussiness.DataModels;

namespace BachelorThesis.ConsoleTest.Parsers
{
    public class SimulationChunksXmlParser
    {
        public List<SimulationChunk> Parse(ProcessInstance process, XElement root)
        {
            var chunks = new List<SimulationChunk>();
            var chunksElement = root.Descendants("Chunks");

            var chunkElements = chunksElement.Elements("Chunk");
            foreach (var chunkElement in chunkElements)
            {
                var chunk = new SimulationChunk();

                var eventElements = chunkElement.Elements("Event");

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
            var eventType = (TransactionEventType)int.Parse(eventElement.Attribute("Type")?.Value);
            var transsactionId = int.Parse(eventElement.Attribute("TransactionId")?.Value);
            var raisedBy = int.Parse(eventElement.Attribute("RaisedById")?.Value);
            var created = DateTime.ParseExact(eventElement.Attribute("Created")?.Value, XmlParsersConfig.DateTimeFormat, CultureInfo.InvariantCulture);

            switch (eventType)
            {
                case TransactionEventType.CompletionChanged:
                    var completionChangedElement = eventElement.Element("CompletionChanged");

                    var oldCompletion = (TransactionCompletion)Enum.Parse(typeof(TransactionCompletion), completionChangedElement?.Attribute("OldCompletion")?.Value);
                    var newCompletion = (TransactionCompletion)Enum.Parse(typeof(TransactionCompletion), completionChangedElement?.Attribute("NewCompletion")?.Value);


                    return new CompletionChangedTransactionEvent(transsactionId, raisedBy, created, oldCompletion, newCompletion);
                case TransactionEventType.InitiatorAssigned:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return null;
        }

    }
}