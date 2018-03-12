using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;

namespace BachelorThesis.Bussiness.DataModels
{
    public class TransactionInstance : IChildrenAware<TransactionInstance>
    {
        private static int nextId;

        public int Id { get; set; }
        public string Identificator { get; set; }
        public float Completion => GetCompletion();
      
        public TransactionCompletion CompletionType { get; set; }
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

        private float GetCompletion()
        {
            switch (CompletionType)
            {
                case TransactionCompletion.None: return 0f;
                case TransactionCompletion.Requested: return 0.25f;
                case TransactionCompletion.Declined:
                    return 0.25f;
                case TransactionCompletion.Quitted:
                    return 0.25f;
                case TransactionCompletion.Promised:
                    return 0.50f;
                case TransactionCompletion.Executed:
                    return 0.6f;
                case TransactionCompletion.Stated:
                    return 0.75f;
                case TransactionCompletion.Rejected:
                    return 0.5f;
                case TransactionCompletion.Stopped:
                    return 0.5f;
                case TransactionCompletion.Accepted:
                    return 1f;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        public string GetIdentificator()
        {
            return Identificator;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Identificator)}: {Identificator}, {nameof(Completion)}: {Completion}, {nameof(CompletionType)}: {CompletionType}, {nameof(ProcessInstanceId)}: {ProcessInstanceId}, {nameof(TransactionKindId)}: {TransactionKindId}, {nameof(InitiatorId)}: {InitiatorId}, {nameof(ExecutorId)}: {ExecutorId}, {nameof(ParentId)}: {ParentId}";
        }
    }
}