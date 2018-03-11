using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;

namespace BachelorThesis.Bussiness.DataModels
{
    public class ProcessInstance
    {
        private static int nextId = 0;

        [Newtonsoft.Json.JsonProperty(Order = 6)]
        private List<TransactionInstance> transactions;

      
        public int Id { get; set; }
        public int ProcessKindId { get; set; }
 
        public float Completion  { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? ExpectedEndTime { get; set; }

        public ProcessInstance() { transactions = new List<TransactionInstance>(); }

        internal ProcessInstance(DateTime startTime, DateTime? expectedEndTime, int processKindId, float completion = 0f)
        {
            Id = Interlocked.Increment(ref nextId);
            ProcessKindId = processKindId;
            StartTime = startTime;
            ExpectedEndTime = expectedEndTime;
            Completion = completion;

            transactions = new List<TransactionInstance>();
        }


        public void AddTransaction(TransactionInstance instance) => transactions.Add(instance);

        public List<TransactionInstance> GetTransactions() => transactions;

        public TransactionInstance GetTransactionById(int id) => transactions.Find(x=> x.Id == id);

        public TransactionInstance GetTransactionByIdentificator(string identificator)
        {
            foreach (var root in transactions)
            {
                var found = TreeStructureHelper.FindByIdentificator(root, identificator);
                if (found != null)
                    return found;
            }

            return null;
        }

        //private TransactionInstance FindByIndetifier(TransactionInstance node, string identifier)
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
    }
}