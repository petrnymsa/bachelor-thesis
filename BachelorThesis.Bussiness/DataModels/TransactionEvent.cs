using System;
using System.Threading;

namespace BachelorThesis.Bussiness.DataModels
{

    public enum TransactionEventType
    {
        CompletionChanged,
        InitiatorAssigned
    }
    public abstract class TransactionEvent
    {
        private static int nextId;

        public int Id { get; set; }
        public TransactionEventType EventType { get; set; }
        public int TransactionEventKindId { get; set; }
        public int TransactionInstanceId { get; set; }
        public int RaisedByActorId { get; set; }
        public DateTime Created { get; set; }

        protected TransactionEvent(TransactionEventType eventType, int transactionInstanceId, int raisedByActorId, DateTime created)
        {
            Id = Interlocked.Increment(ref nextId);
            EventType = eventType;
            //TransactionEventKindId = transactionEventKindId;
            TransactionInstanceId = transactionInstanceId;
            RaisedByActorId = raisedByActorId;
            Created = created;
        }
    }

    public class CompletionChangedTransactionEvent : TransactionEvent
    {
        public TransactionCompletion OldCompletion { get; }
        public TransactionCompletion NewCompletion { get; }

        public CompletionChangedTransactionEvent(int transactionInstanceId, int raisedByActorId, DateTime created, TransactionCompletion oldCompletion, TransactionCompletion newCompletion) 
            : base(TransactionEventType.CompletionChanged,transactionInstanceId, raisedByActorId, created)
        {
            OldCompletion = oldCompletion;
            NewCompletion = newCompletion;
        }
    }

    public class InitiatorAssigned : TransactionEvent
    {
        public int IntiatorId { get; set; }

        public InitiatorAssigned(int transactionInstanceId, int raisedByActorId, DateTime created, int initiatorId) 
            : base(TransactionEventType.InitiatorAssigned, transactionInstanceId, raisedByActorId, created)
        {
            this.IntiatorId = initiatorId;
        }
    }
}