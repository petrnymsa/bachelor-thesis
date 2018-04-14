using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using BachelorThesis.Business.DataModels;

namespace BachelorThesis.Business.Parsers
{
    public class ProcessKindXmlParser
    {

        private static XElement CreateTransactionKindElement(TransactionKind kind)
        {
            var kindElement = new XElement(XmlParsersConfig.ElementTransactionKind);
            kindElement.Add(new XAttribute(XmlParsersConfig.AttributeId, kind.Id));
            kindElement.Add(new XAttribute(XmlParsersConfig.AttributeIdentificator, kind.Identificator));
            kindElement.Add(new XAttribute(XmlParsersConfig.AttributeFirstName, kind.Name));
            kindElement.Add(new XAttribute(XmlParsersConfig.AttributeOptimisticTimeEstimate, kind.OptimisticTimeEstimate));
            kindElement.Add(new XAttribute(XmlParsersConfig.AttributeNormalTimeEstimate, kind.NormalTimeEstimate));
            kindElement.Add(new XAttribute(XmlParsersConfig.AttributePesimisticTimeEstimate, kind.PesimisticTimeEstimate));
            kindElement.Add(new XAttribute(XmlParsersConfig.AttributeProcessKindId, kind.ProcessKindId));
            kindElement.Add(new XAttribute(XmlParsersConfig.AttributeInitiatorRoleId, kind.InitiatorKindId));
            kindElement.Add(new XAttribute(XmlParsersConfig.AttributeExecutorRoleId, kind.ExecutorKindId));

            return kindElement;
        }

        public XDocument CreateDocument(ProcessKind process, List<ActorRole> actors)
        {
            var processElement = new XElement(XmlParsersConfig.ElementProcessKind);
            processElement.Add(new XAttribute(XmlParsersConfig.AttributeId, process.Id));
            processElement.Add(new XAttribute(XmlParsersConfig.AttributeFirstName, process.Name));

            // actors
            var actorsElement = new XElement(XmlParsersConfig.ElementActorRoles, 
                from actor in actors
                    select new XElement(XmlParsersConfig.ElementActorRole, 
                        new XAttribute(XmlParsersConfig.AttributeId, actor.Id),
                        new XAttribute(XmlParsersConfig.AttributeName, actor.Name)));

            processElement.Add(actorsElement);
        

            // transactions 
            var transactionsElement = new XElement(XmlParsersConfig.ElementTransactions);

            CreateTransactionElements(process, transactionsElement);

            processElement.Add(transactionsElement);

            // links
            var linksElement = new XElement(XmlParsersConfig.ElementLinks);

            CreateLinkElements(process, linksElement);

            processElement.Add(linksElement);

            return new XDocument(processElement);
        }

        public SimulationModelDefinition ParseDefinition(XDocument doc)
        {
            var result = new SimulationModelDefinition();

            var processId = int.Parse(doc.Root.Attribute(XmlParsersConfig.AttributeId).Value);
            var processName = doc.Root.Attribute(XmlParsersConfig.AttributeName).Value;

            var processKind = new ProcessKind {Id = processId, Name = processName };

            result.ActorRoles = ParseActorRoles(doc.Root);

            ParseTransactions(doc.Root).ForEach(x => processKind.AddTransaction(x));

            ParseTransactionLinks(doc.Root).ForEach(x => processKind.AddTransactionLink(x));

            result.ProcessKind = processKind;
            return result;
        }

        private List<TransactionLink> ParseTransactionLinks(XElement root)
        {
            var result = new List<TransactionLink>();

            var linksElement = root.Element(XmlParsersConfig.ElementLinks);
            var linkElements = linksElement.Elements(XmlParsersConfig.ElementTransactionLink);

            foreach (var xElement in linkElements)
            {
                var link = new TransactionLink()
                {
                    Id = int.Parse(xElement.Attribute(XmlParsersConfig.AttributeId).Value),
                    Type = (TransactionLinkType)Enum.Parse(typeof(TransactionLinkType), xElement.Attribute(XmlParsersConfig.AttributeType).Value),
                    SourceTransactionKindId = int.Parse(xElement.Attribute(XmlParsersConfig.AttributeSourceTransactionId).Value),
                    DestinationTransactionKindId = int.Parse(xElement.Attribute(XmlParsersConfig.AttributeDestinationTransactionId).Value),
                    SourceCompletion = (TransactionCompletion)Enum.Parse(typeof(TransactionCompletion),
                        xElement.Attribute(XmlParsersConfig.AttributeSourceCompletion).Value),
                    DestinationCompletion = (TransactionCompletion)Enum.Parse(typeof(TransactionCompletion),
                        xElement.Attribute(XmlParsersConfig.AttributeDestinationCompletion).Value),

                };

                var sourceCardinalityElement = xElement.Element(XmlParsersConfig.ElementSourceCardinality);
                var destinationCardinalityElement = xElement.Element(XmlParsersConfig.ElementDestinationCardinality);

                var sourceCardinality = (TransactionLinkCardinality)Enum.Parse(typeof(TransactionLinkCardinality),
                    sourceCardinalityElement.Attribute(XmlParsersConfig.AttributeCardinality).Value);

                var destinationCardinality = (TransactionLinkCardinality)Enum.Parse(typeof(TransactionLinkCardinality),
                    sourceCardinalityElement.Attribute(XmlParsersConfig.AttributeCardinality).Value);

                link.SourceCardinality = sourceCardinality;
                link.DestinationCardinality = destinationCardinality;

                if (sourceCardinality == TransactionLinkCardinality.Interval)
                {
                    var intervalElement = sourceCardinalityElement.Element(XmlParsersConfig.ElementInterval);
                    var min = int.Parse(intervalElement.Attribute(XmlParsersConfig.AttributeMin).Value);
                    var max = int.Parse(intervalElement.Attribute(XmlParsersConfig.AttributeMax).Value);

                    link.SourceCardinalityInterval = new CardinalityInterval(min,max);
                }

                if (destinationCardinality == TransactionLinkCardinality.Interval)
                {
                    var intervalElement = destinationCardinalityElement.Element(XmlParsersConfig.ElementInterval);
                    var min = int.Parse(intervalElement.Attribute(XmlParsersConfig.AttributeMin).Value);
                    var max = int.Parse(intervalElement.Attribute(XmlParsersConfig.AttributeMax).Value);

                    link.DestinationCardinalityInterval = new CardinalityInterval(min,max);
                }

                result.Add(link);

            }

            return result;
        }

        private List<TransactionKind> ParseTransactions(XElement root)
        {
            var result = new List<TransactionKind>();
            var transactionsElement = root.Element(XmlParsersConfig.ElementTransactions);
            var transactionElements = transactionsElement.Elements(XmlParsersConfig.ElementTransactionKind);

            foreach (var transactionElement in transactionElements)
            {
                var transactionKind = ParseTransactionKind(transactionElement);

                ParseTransactionChildren(transactionElement, transactionKind);

                result.Add(transactionKind);
            }

            return result;
        }

        private void ParseTransactionChildren(XElement xElement, TransactionKind transactionKind)
        {
            var childElements = xElement.Elements(XmlParsersConfig.ElementTransactionKind);
            foreach (var childElement in childElements)
            {
                var childKind = ParseTransactionKind(childElement);
                transactionKind.AddChild(childKind);

                ParseTransactionChildren(childElement, childKind);
            }
        }

        private TransactionKind ParseTransactionKind(XElement xElement)
        {
            var transaction = new TransactionKind()
            {
                Id = int.Parse(xElement.Attribute(XmlParsersConfig.AttributeId).Value),
                Identificator = xElement.Attribute(XmlParsersConfig.AttributeIdentificator).Value,
                Name = xElement.Attribute(XmlParsersConfig.AttributeName).Value,
                ProcessKindId = int.Parse(xElement.Attribute(XmlParsersConfig.AttributeProcessKindId).Value),
                InitiatorKindId = xElement.Attribute(XmlParsersConfig.AttributeInitiatorRoleId).Value.ToNullableInt(),
                ExecutorKindId = xElement.Attribute(XmlParsersConfig.AttributeExecutorRoleId).Value.ToNullableInt(),
                ParentId = xElement.Attribute(XmlParsersConfig.AttributeParentid).Value.ToNullableInt()
            };

            var optimisticTime = double.Parse(xElement.Attribute(XmlParsersConfig.AttributeOptimisticTimeEstimate).Value);
            var normalTime = double.Parse(xElement.Attribute(XmlParsersConfig.AttributeNormalTimeEstimate).Value);
            var pesimisticTime = double.Parse(xElement.Attribute(XmlParsersConfig.AttributePesimisticTimeEstimate).Value);

            transaction.SetTimeEstimate(optimisticTime,normalTime,pesimisticTime);

            return transaction;
        }

        private List<ActorRole> ParseActorRoles(XElement root)
        {
            var rolesElement = root.Element(XmlParsersConfig.ElementActorRoles);
            var roleElements = rolesElement.Elements(XmlParsersConfig.ElementActorRole);

            return (from roleElement in roleElements
                let id = int.Parse(roleElement.Attribute(XmlParsersConfig.AttributeId).Value)
                let name = roleElement.Attribute(XmlParsersConfig.AttributeName).Value
                select new ActorRole {Id = id, Name = name}).ToList();

        }

        private static void CreateLinkElements(ProcessKind process, XElement linksElement)
        {
            foreach (var link in process.GetLinks())
            {
                var linkElement = new XElement(XmlParsersConfig.ElementTransactionLink);
                linkElement.Add(new XAttribute(XmlParsersConfig.AttributeId, link.Id));
                linkElement.Add(new XAttribute(XmlParsersConfig.AttributeType, link.Type));
                linkElement.Add(new XAttribute(XmlParsersConfig.AttributeSourceTransactionId, link.SourceTransactionKindId));
                linkElement.Add(new XAttribute(XmlParsersConfig.AttributeDestinationTransactionId, link.DestinationTransactionKindId));
                linkElement.Add(new XAttribute(XmlParsersConfig.AttributeSourceCompletion, link.SourceCompletion));
                linkElement.Add(new XAttribute(XmlParsersConfig.AttributeDestinationCompletion, link.DestinationCompletion));

                var sourceCardinality = new XElement(XmlParsersConfig.ElementSourceCardinality, new XAttribute(XmlParsersConfig.AttributeCardinality, link.SourceCardinality));
                var destinationCardinality = new XElement(XmlParsersConfig.ElementDestinationCardinality, new XAttribute(XmlParsersConfig.AttributeCardinality, link.DestinationCardinality));

                if (link.SourceCardinality == TransactionLinkCardinality.Interval)
                {
                    sourceCardinality.Add(new XElement(XmlParsersConfig.ElementInterval,
                        new XAttribute(XmlParsersConfig.AttributeMin, link.SourceCardinalityInterval.Min),
                        new XAttribute(XmlParsersConfig.AttributeMax, link.SourceCardinalityInterval.Max)));
                }

                if (link.DestinationCardinality == TransactionLinkCardinality.Interval)
                {
                    destinationCardinality.Add(new XElement(XmlParsersConfig.ElementInterval,
                        new XAttribute(XmlParsersConfig.AttributeMin, link.DestinationCardinalityInterval.Min),
                        new XAttribute(XmlParsersConfig.AttributeMax, link.DestinationCardinalityInterval.Max)));
                }

                linkElement.Add(sourceCardinality);
                linkElement.Add(destinationCardinality);

                linksElement.Add(linkElement);

            }
        }

        private static void CreateTransactionElements(ProcessKind process, XElement transactionsElement)
        {
            foreach (var kind in process.GetTransactions())
            {
                var rootElement = CreateTransactionKindElement(kind);

                TreeStructureHelper.Traverse(kind, rootElement, (node, element) =>
                {
                    var childElement = CreateTransactionKindElement(node);
                    element.Add(childElement);
                });

                transactionsElement.Add(rootElement);
            }
        }
    }
}