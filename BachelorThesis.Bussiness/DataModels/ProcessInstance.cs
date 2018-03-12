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

        public TransactionInstance GetTransactionById(int id)
        {
            foreach (var instance in transactions)
            {
                var found = TreeStructureHelper.Find(instance, (node) => node.Id == id);
                if (found != null)
                    return found;
            }

            return null;
        }

        public TransactionInstance GetTransactionByIdentificator(string identificator)
        {
            foreach (var instance in transactions)
            {
                var found = TreeStructureHelper.Find(instance, (node) => string.Equals(node.Identificator, identificator, StringComparison.InvariantCulture));
                if (found != null)
                    return found;
            }

            return null;
        }
    }
}