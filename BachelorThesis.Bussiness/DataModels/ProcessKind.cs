using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BachelorThesis.Bussiness.DataModels
{
    [DataContract]
    public class ProcessKind
    {
        private static int nextId;

        public int Id { get; }
        public string Name { get; set; }

        [JsonProperty(Order = 3)]
        [DataMember]
        private List<TransactionKind> transactions;

        [JsonProperty(Order = 4)]
        [DataMember]
        private List<TransactionLink> links;

        public ProcessKind() { Id = Interlocked.Increment(ref nextId); }

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

        public void AddTransactions(params TransactionKind[] transactionParams)
        {
            foreach (var kind in transactionParams)
            {
                kind.ProcessKindId = Id;
                transactions.Add(kind);
            }
        }

        public List<TransactionKind> GetTransactions() => transactions;

        public TransactionKind GetTransactionById(int id) => transactions.Find(x => x.Id == id);

        public TransactionKind GetTransactionByIdentifier(string identifier)
        {
            foreach (var root in transactions)
            {
                var found = TreeStructureHelper.FindByIdentificator(root, identifier);
                if (found != null)
                    return found;
            }

            return null;
        }

        //private TransactionKind FindByIndetifier(TransactionKind node, string identifier)
        //{
        //    if (node == null)
        //        return null;

        //    if (string.Equals(node.Identificator, identifier, StringComparison.InvariantCulture))
        //        return node;

        //    foreach (var child in node.GetChildren())
        //    {
        //        var found = FindByIndetifier(child, identifier);
        //        if (found != null)
        //            return found;
        //    }

        //    return null;
        //}

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

        //public void AddProcessInstance(ProcessInstance process)
        //{
        //    process.ProcessKindId = Id;
        //    processes.Add(process);
        //}

        //public void RemoveProcessInstance(ProcessInstance process)
        //{
        //    processes.Remove(process);
        //}

        //public List<ProcessInstance> GetProcessInstances() => processes;
      

        public ProcessInstance NewInstance(DateTime startTime, float completion = 0f)
        {
            var process = new ProcessInstance(startTime, null,Id, completion);
            foreach (var kind in transactions)
            {
                var instance = kind.NewInstance(process.Id);
                AddChild(kind, instance, process);
                process.AddTransaction(instance);
          
            }

            return process;
        }

        private void AddChild(TransactionKind node, TransactionInstance instance, ProcessInstance process)
        {
            if(node == null)
                return;

            foreach (var child in node.GetChildren())
            {
                var childInstance = child.NewInstance(process.ProcessKindId);
                instance.AddChild(childInstance);
                
                AddChild(child,childInstance,process);
            }
        }
    }
}