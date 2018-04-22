using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using Newtonsoft.Json;

namespace BachelorThesis.Business.DataModels
{
    [DataContract]
    public class ProcessKind
    {
        private static int nextId;
        [JsonProperty]
        public int Id { get; set; }
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty(Order = 3)]
        [DataMember]
        private List<TransactionKind> transactions;

        [JsonProperty(Order = 4)]
        [DataMember]
        private List<TransactionLink> links;
        [JsonProperty]
        public List<ActorRole> ActorRoles { get; set; }

        public ProcessKind()
        {
            Id = Interlocked.Increment(ref nextId);
            transactions = new List<TransactionKind>();
            ActorRoles = new List<ActorRole>();
            links = new List<TransactionLink>();

        }

        public ProcessKind(string name)
        {
            this.Name = name;
            transactions = new List<TransactionKind>();
            links = new List<TransactionLink>();
            Id = Interlocked.Increment(ref nextId);
        }

        public void AddTransaction(TransactionKind transaction)
        {
            transaction.ProcessKindId = Id;
            transactions.Add(transaction);
        }


        public List<TransactionKind> GetTransactions() => transactions;

        public TransactionKind GetTransactionById(int id)
        {
            foreach (var root in transactions)
            {
                var found = TreeStructureHelper.Find(root, x => x.Id == id);
                if (found != null)
                    return found;
            }

            return null;
        }

        public TransactionKind GetTransactionByIdentifier(string identifier)
        {
            foreach (var root in transactions)
            {
                var found = TreeStructureHelper.Find(root, x => string.Equals(x.Identificator, identifier, StringComparison.InvariantCulture));
                if (found != null)
                    return found;
            }

            return null;
        }

        public void AddTransactionLink(TransactionKind sourceTransaction, TransactionKind destinationTransaction,TransactionCompletion sourceCompletion, TransactionCompletion destinationCompletion, TransactionLinkType linkType)
        {
            links.Add(new TransactionLink
            {
                SourceTransactionKindId = sourceTransaction.Id,
                DestinationTransactionKindId = destinationTransaction.Id,
                SourceCompletion = sourceCompletion,
                DestinationCompletion = destinationCompletion,
                Type = linkType
            });
        }

        public void AddTransactionLink(TransactionLink link)
        {
            links.Add(link);
        }

        public List<TransactionLink> GetLinks() => links;

        public List<TransactionLink> GetLinksAsSourceForTransaction(int transactionKindId)
        {
            return links.Where(x => x.SourceTransactionKindId == transactionKindId).ToList();
        }

        //public ProcessInstance NewInstance(DateTime startTime, float completion = 0f)
        //{
        //    var process = new ProcessInstance(startTime, null,Id, completion);
        //    foreach (var kind in transactions)
        //    {
        //        var instance = kind.NewInstance(process.Id);
        //        AddChild(kind, instance, process);
        //        process.AddTransaction(instance);
          
        //    }

        //    return process;
        //}

//        private void AddChild(TransactionKind node, TransactionInstance instance, ProcessInstance process)
//        {
//            if(node == null)
//                return;
//
//            foreach (var child in node.GetChildren())
//            {
//                var childInstance = child.NewInstance(process.ProcessKindId);
//                instance.AddChild(childInstance);
//                
//                AddChild(child,childInstance,process);
//            }
//        }

        public override string ToString()
        {
            return $"{nameof(transactions)}: {transactions.Count}, {nameof(links)}: {links.Count}, {nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(ActorRoles)}: {ActorRoles.Count}";
        }
    }
}