using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;

namespace BachelorThesis.Business.DataModels
{
    public class TransactionInstance : IChildrenAware<TransactionInstance>
    {
        private static int nextId;

        public int Id { get; set; }
        public string Identificator { get; set; }
        public float CompletionNumber => GetCompletion();
      
        public TransactionCompletion Completion { get; set; }
        public int ProcessInstanceId { get; set; }
        public int TransactionKindId { get; set; }
        public int? InitiatorId { get; set; }
        public int? ExecutorId { get; set; }
        public int? ParentId { get; set; }

        [JsonProperty(Order = 9)]
        private readonly List<TransactionInstance> children;

        public TransactionInstance()
        {
            Id = Interlocked.Increment(ref nextId);
            children = new List<TransactionInstance>();
        }

        public void AddChild(TransactionInstance child)
        {
            child.ParentId = Id;
            children.Add(child);
        }

        public List<TransactionInstance> GetChildren() => children;

        private float GetCompletion() => Completion.ToProgressCoefficient();


        public string GetIdentificator()
        {
            return Identificator;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Identificator)}: {Identificator}, {nameof(CompletionNumber)}: {CompletionNumber}, {nameof(Completion)}: {Completion}, {nameof(ProcessInstanceId)}: {ProcessInstanceId}, {nameof(TransactionKindId)}: {TransactionKindId}, {nameof(InitiatorId)}: {InitiatorId}, {nameof(ExecutorId)}: {ExecutorId}, {nameof(ParentId)}: {ParentId}";
        }
    }
}