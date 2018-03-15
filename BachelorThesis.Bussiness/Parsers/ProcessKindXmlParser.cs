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
        private const string AttributeInitiatorKindId = "InitiatorKindId";
        private const string AttributeExecutorKindId = "ExecutorKindId";
        private const string ElementProcessKind = "ProcessKind";
        private const string ElementActors = "Actors";
        private const string ElementActorKind = "ActorKind";
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
            kindElement.Add(new XAttribute(AttributeInitiatorKindId, kind.InitiatorKindId));
            kindElement.Add(new XAttribute(AttributeExecutorKindId, kind.ExecutorKindId));

            return kindElement;
        }

        public static XDocument CreateDocument(ProcessKind process, List<ActorKind> actors)
        {
            var processElement = new XElement(ElementProcessKind);
            processElement.Add(new XAttribute(AttributeId, process.Id));
            processElement.Add(new XAttribute(AttributeFirstName, process.Name));

            // actors
            var actorsElement = new XElement(ElementActors, 
                from actor in actors
                    select new XElement(ElementActorKind, 
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
}