using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using BachelorThesis.Bussiness.DataModels;

namespace BachelorThesis.Bussiness.Parsers
{
    public class ProcessKindXmlParser
    {
        private const string ElementTransactionKind = "TransactionKind";
        private const string AttributeId = "Id";
        private const string AttributeIdentificator = "Identificator";
        private const string AttributeFirstName = "FirstName";
        private const string AttributeOptimisticTimeEstimate = "OptimisticTimeEstimate";
        private const string AttributeNormalTimeEstimate = "NormalTimeEstimate";
        private const string AttributePesimisticTimeEstimate = "PesimisticTimeEstimate";
        private const string AttributeProcessKindId = "ProcessKindId";
        private const string AttributeInitiatorRoleId = "InitiatorRoleId";
        private const string AttributeExecutorRoleId = "ExecutorRoleId";
        private const string ElementProcessKind = "ProcessKind";
        private const string ElementActorRoles = "ActorRoles";
        private const string ElementActorRole = "ActorRole";
        private const string AttributeName = "Name";
        private const string ElementTransactions = "Transactions";
        private const string ElementLinks = "Links";
        private const string ElementTransactionLink = "TransactionLink";
        private const string AttributeType = "Type";
        private const string AttributeSourceTransactionId = "SourceTransactionId";
        private const string AttributeDestinationTransactionId = "DestinationTransactionId";
        private const string AttributeSourceCompletion = "SourceCompletion";
        private const string AttributeDestinationCompletion = "DestinationCompletion";
        private const string ElementSourceCardinality = "SourceCardinality";
        private const string AttributeCardinality = "Cardinality";
        private const string ElementDestinationCardinality = "DestinationCardinality";
        private const string ElementInterval = "Interval";
        private const string AttributeMin = "Min";
        private const string AttributeMax = "Max";
        private const string AttributeParentid = "ParentId";

        private static XElement CreateTransactionKindElement(TransactionKind kind)
        {
            var kindElement = new XElement(ElementTransactionKind);
            kindElement.Add(new XAttribute(AttributeId, kind.Id));
            kindElement.Add(new XAttribute(AttributeIdentificator, kind.Identificator));
            kindElement.Add(new XAttribute(AttributeFirstName, kind.Name));
            kindElement.Add(new XAttribute(AttributeOptimisticTimeEstimate, kind.OptimisticTimeEstimate));
            kindElement.Add(new XAttribute(AttributeNormalTimeEstimate, kind.NormalTimeEstimate));
            kindElement.Add(new XAttribute(AttributePesimisticTimeEstimate, kind.PesimisticTimeEstimate));
            kindElement.Add(new XAttribute(AttributeProcessKindId, kind.ProcessKindId));
            kindElement.Add(new XAttribute(AttributeInitiatorRoleId, kind.InitiatorKindId));
            kindElement.Add(new XAttribute(AttributeExecutorRoleId, kind.ExecutorKindId));

            return kindElement;
        }

        public XDocument CreateDocument(ProcessKind process, List<ActorRole> actors)
        {
            var processElement = new XElement(ElementProcessKind);
            processElement.Add(new XAttribute(AttributeId, process.Id));
            processElement.Add(new XAttribute(AttributeFirstName, process.Name));

            // actors
            var actorsElement = new XElement(ElementActorRoles, 
                from actor in actors
                    select new XElement(ElementActorRole, 
                        new XAttribute(AttributeId, actor.Id),
                        new XAttribute(AttributeName, actor.Name)));

            processElement.Add(actorsElement);
        

            // transactions 
            var transactionsElement = new XElement(ElementTransactions);

            CreateTransactionElements(process, transactionsElement);

            processElement.Add(transactionsElement);

            // links
            var linksElement = new XElement(ElementLinks);

            CreateLinkElements(process, linksElement);

            processElement.Add(linksElement);

            return new XDocument(processElement);
        }

        public SimulationModelDefinition ParseDefinition(XDocument doc)
        {
            var result = new SimulationModelDefinition();

            var processId = int.Parse(doc.Root.Attribute(AttributeId).Value);
            var processName = doc.Root.Attribute(AttributeName).Value;

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

            var linksElement = root.Element(ElementLinks);
            var linkElements = linksElement.Elements(ElementTransactionLink);

            foreach (var xElement in linkElements)
            {
                var link = new TransactionLink()
                {
                    Id = int.Parse(xElement.Attribute(AttributeId).Value),
                    Type = (TransactionLinkType)Enum.Parse(typeof(TransactionLinkType), xElement.Attribute(AttributeType).Value),
                    SourceTransactionKindId = int.Parse(xElement.Attribute(AttributeSourceTransactionId).Value),
                    DestinationTransactionKindId = int.Parse(xElement.Attribute(AttributeDestinationTransactionId).Value),
                    SourceCompletion = (TransactionCompletion)Enum.Parse(typeof(TransactionCompletion),
                        xElement.Attribute(AttributeSourceCompletion).Value),
                    DestinationCompletion = (TransactionCompletion)Enum.Parse(typeof(TransactionCompletion),
                        xElement.Attribute(AttributeDestinationCompletion).Value),

                };

                var sourceCardinalityElement = xElement.Element(ElementSourceCardinality);
                var destinationCardinalityElement = xElement.Element(ElementDestinationCardinality);

                var sourceCardinality = (TransactionLinkCardinality)Enum.Parse(typeof(TransactionLinkCardinality),
                    sourceCardinalityElement.Attribute(AttributeCardinality).Value);

                var destinationCardinality = (TransactionLinkCardinality)Enum.Parse(typeof(TransactionLinkCardinality),
                    sourceCardinalityElement.Attribute(AttributeCardinality).Value);

                link.SourceCardinality = sourceCardinality;
                link.DestinationCardinality = destinationCardinality;

                if (sourceCardinality == TransactionLinkCardinality.Interval)
                {
                    var intervalElement = sourceCardinalityElement.Element(ElementInterval);
                    var min = int.Parse(intervalElement.Attribute(AttributeMin).Value);
                    var max = int.Parse(intervalElement.Attribute(AttributeMax).Value);

                    link.SourceCardinalityInterval = new CardinalityInterval(min,max);
                }

                if (destinationCardinality == TransactionLinkCardinality.Interval)
                {
                    var intervalElement = destinationCardinalityElement.Element(ElementInterval);
                    var min = int.Parse(intervalElement.Attribute(AttributeMin).Value);
                    var max = int.Parse(intervalElement.Attribute(AttributeMax).Value);

                    link.DestinationCardinalityInterval = new CardinalityInterval(min,max);
                }

                result.Add(link);

            }

            return result;
        }

        private List<TransactionKind> ParseTransactions(XElement root)
        {
            var result = new List<TransactionKind>();
            var transactionsElement = root.Element(ElementTransactions);
            var transactionElements = transactionsElement.Elements(ElementTransactionKind);

            foreach (var transactionElement in transactionElements)
            {
                var transactionKind = ParseTransactionKind(transactionElement);

                TreeStructureHelper.Traverse(transactionKind, transactionElement, (kind, node) =>
                {
                    var transactionChild = ParseTransactionKind(node);
                    kind.AddChild(transactionChild);
;                });

                result.Add(transactionKind);
            }

            return result;
        }

        private TransactionKind ParseTransactionKind(XElement xElement)
        {
            var transaction = new TransactionKind()
            {
                Id = int.Parse(xElement.Attribute(AttributeId).Value),
                Identificator = xElement.Attribute(AttributeIdentificator).Value,
                Name = xElement.Attribute(AttributeName).Value,
                ProcessKindId = int.Parse(xElement.Attribute(AttributeProcessKindId).Value),
                InitiatorKindId = int.Parse(xElement.Attribute(AttributeInitiatorRoleId).Value),
                ExecutorKindId = int.Parse(xElement.Attribute(AttributeExecutorRoleId).Value),
                ParentId = int.Parse(xElement.Attribute(AttributeParentid).Value)
            };

            var optimisticTime = double.Parse(xElement.Attribute(AttributeOptimisticTimeEstimate).Value);
            var normalTime = double.Parse(xElement.Attribute(AttributeNormalTimeEstimate).Value);
            var pesimisticTime = double.Parse(xElement.Attribute(AttributePesimisticTimeEstimate).Value);

            transaction.SetTimeEstimate(optimisticTime,normalTime,pesimisticTime);

            return transaction;
        }

        private List<ActorRole> ParseActorRoles(XElement root)
        {
            var rolesElement = root.Element(ElementActorRoles);
            var roleElements = rolesElement.Elements(ElementActorRole);

            return (from roleElement in roleElements
                let id = int.Parse(roleElement.Attribute(AttributeId).Value)
                let name = roleElement.Attribute(AttributeName).Value
                select new ActorRole {Id = id, Name = name}).ToList();

        }

        private static void CreateLinkElements(ProcessKind process, XElement linksElement)
        {
            foreach (var link in process.GetLinks())
            {
                var linkElement = new XElement(ElementTransactionLink);
                linkElement.Add(new XAttribute(AttributeId, link.Id));
                linkElement.Add(new XAttribute(AttributeType, link.Type));
                linkElement.Add(new XAttribute(AttributeSourceTransactionId, link.SourceTransactionKindId));
                linkElement.Add(new XAttribute(AttributeDestinationTransactionId, link.DestinationTransactionKindId));
                linkElement.Add(new XAttribute(AttributeSourceCompletion, link.SourceCompletion));
                linkElement.Add(new XAttribute(AttributeDestinationCompletion, link.DestinationCompletion));

                var sourceCardinality = new XElement(ElementSourceCardinality, new XAttribute(AttributeCardinality, link.SourceCardinality));
                var destinationCardinality = new XElement(ElementDestinationCardinality, new XAttribute(AttributeCardinality, link.DestinationCardinality));

                if (link.SourceCardinality == TransactionLinkCardinality.Interval)
                {
                    sourceCardinality.Add(new XElement(ElementInterval,
                        new XAttribute(AttributeMin, link.SourceCardinalityInterval.Min),
                        new XAttribute(AttributeMax, link.SourceCardinalityInterval.Max)));
                }

                if (link.DestinationCardinality == TransactionLinkCardinality.Interval)
                {
                    destinationCardinality.Add(new XElement(ElementInterval,
                        new XAttribute(AttributeMin, link.DestinationCardinalityInterval.Min),
                        new XAttribute(AttributeMax, link.DestinationCardinalityInterval.Max)));
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

    public class SimulationModelDefinition
    {
        public ProcessKind ProcessKind { get; set; }
        public List<ActorRole> ActorRoles { get; set; }
    }
}