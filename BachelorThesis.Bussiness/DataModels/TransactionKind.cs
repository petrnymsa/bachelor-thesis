using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;

namespace BachelorThesis.Bussiness.DataModels
{
    public class TransactionKind : IChildrenAware<TransactionKind>
    {
        private static int nextId = 0;

        public int Id { get; set; }
        public int ProcessKindId { get; set; }
        public string Identificator { get; set; }
        public string Name { get; set; }

        public double OptimisticTimeEstimate { get; private set; }
        public double NormalTimeEstimate { get; private set; }
        public double PesimisticTimeEstimate { get; private set; }
        public double ExpectedTimeEstimate { get; private set; }

        public int InitiatorKindId { get; set; }
        public int ExecutorKindId { get; set; }

        public int? ParentId { get; set; }

        [JsonProperty(Order = 10)]
        private List<TransactionKind> children;

        public void AddChild(TransactionKind child)
        {
            child.ParentId = Id;
            children.Add(child);
        }

        public List<TransactionKind> GetChildren() => children;

        public TransactionKind(string identificator, string name, int processKindId, int initiatorKindId, int executorKindId)
        {
            Id = Interlocked.Increment(ref nextId);
            Identificator = identificator;
            Name = name;
            ProcessKindId = processKindId;
            InitiatorKindId = initiatorKindId;
            ExecutorKindId = executorKindId;
            ExpectedTimeEstimate = 0;

            children = new List<TransactionKind>();
        }

        public void SetTimeEstimate(double optimistic, double normal, double pesimistic)
        {
            OptimisticTimeEstimate = optimistic;
            NormalTimeEstimate = normal;
            PesimisticTimeEstimate = pesimistic;

            ExpectedTimeEstimate = (optimistic + 4 * normal + pesimistic) / 6;
        }

        public TransactionInstance NewInstance(int processInstanceId)
        {
            return new TransactionInstance
            {
                Completion = TransactionCompletion.None,
                TransactionKindId = this.Id,
                ProcessInstanceId = processInstanceId,
                Identificator = this.Identificator
            };
        }

        public string GetIdentificator()
        {
            return Identificator;
        }
    }
}