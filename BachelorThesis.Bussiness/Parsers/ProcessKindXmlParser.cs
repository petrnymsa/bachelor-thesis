using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using BachelorThesis.Bussiness.DataModels;

namespace BachelorThesis.Bussiness.Parsers
{
    public class ProcessKindXmlParser
    {
        private static XElement CreateTransactionKindElement(TransactionKind kind)
        {
            var kindElement = new XElement("TransactionKind");
            kindElement.Add(new XAttribute("Id", kind.Id));
            kindElement.Add(new XAttribute("Identificator", kind.Identificator));
            kindElement.Add(new XAttribute("FirstName", kind.Name));
            kindElement.Add(new XAttribute("OptimisticTimeEstimate", kind.OptimisticTimeEstimate));
            kindElement.Add(new XAttribute("NormalTimeEstimate", kind.NormalTimeEstimate));
            kindElement.Add(new XAttribute("PesimisticTimeEstimate", kind.PesimisticTimeEstimate));
            kindElement.Add(new XAttribute("ProcessKindId", kind.ProcessKindId));
            kindElement.Add(new XAttribute("InitiatorKindId", kind.InitiatorKindId));
            kindElement.Add(new XAttribute("ExecutorKindId", kind.ExecutorKindId));

            return kindElement;
        }

        public static XDocument CreateDocument(ProcessKind process, List<ActorKind> actors)
        {
            var processElement = new XElement("ProcessKind");
            processElement.Add(new XAttribute("Id", process.Id));
            processElement.Add(new XAttribute("FirstName", process.Name));

            // actors
            var actorsElement = new XElement("Actors", 
                from actor in actors
                    select new XElement("ActorKind", 
                        new XAttribute("Id", actor.Id),
                        new XAttribute("Name", actor.Name)));

            processElement.Add(actorsElement);
        

            // transactions 
            var transactionsElement = new XElement("Transactions");

            CreateTransactionElements(process, transactionsElement);

            processElement.Add(transactionsElement);

            // links
            var linksElement = new XElement("Links");

            CreateLinkElements(process, linksElement);

            processElement.Add(linksElement);

            return new XDocument(processElement);
        }

        private static void CreateLinkElements(ProcessKind process, XElement linksElement)
        {
            foreach (var link in process.GetLinks())
            {
                var linkElement = new XElement("TransactionLink");
                linkElement.Add(new XAttribute("Id", link.Id));
                linkElement.Add(new XAttribute("Type", link.Type));
                linkElement.Add(new XAttribute("SourceTransactionId", link.SourceTransactionKindId));
                linkElement.Add(new XAttribute("DestinationTransactionId", link.DestinationTransactionKindId));
                linkElement.Add(new XAttribute("SourceCompletion", link.SourceCompletion));
                linkElement.Add(new XAttribute("DestinationCompletion", link.DestinationCompletion));

                var sourceCardinality = new XElement("SourceCardinality", new XAttribute("Cardinality", link.SourceCardinality));
                var destinationCardinality = new XElement("DestinationCardinality", new XAttribute("Cardinality", link.DestinationCardinality));

                if (link.SourceCardinality == TransactionLinkCardinality.Interval)
                {
                    sourceCardinality.Add(new XElement("Interval",
                        new XAttribute("Min", link.SourceCardinalityInterval.Min),
                        new XAttribute("Max", link.SourceCardinalityInterval.Max)));
                }

                if (link.DestinationCardinality == TransactionLinkCardinality.Interval)
                {
                    destinationCardinality.Add(new XElement("Interval",
                        new XAttribute("Min", link.DestinationCardinalityInterval.Min),
                        new XAttribute("Max", link.DestinationCardinalityInterval.Max)));
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