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
        public int TransactionInstanceId { get; set; }
        public int RaisedByActorId { get; set; }
        public DateTime Created { get; set; }

        protected TransactionEvent(TransactionEventType eventType, int transactionInstanceId, int raisedByActorId, DateTime created)
        {
            Id = Interlocked.Increment(ref nextId);
            EventType = eventType;
            TransactionInstanceId = transactionInstanceId;
            RaisedByActorId = raisedByActorId;
            Created = created;
        }

        public abstract void DoTransactionAction(TransactionInstance instance);
    }

    public class CompletionChangedTransactionEvent : TransactionEvent
    {
        public TransactionCompletion Completion { get; }

        public CompletionChangedTransactionEvent(int transactionInstanceId, int raisedByActorId, DateTime created, TransactionCompletion completion)
            : base(TransactionEventType.CompletionChanged, transactionInstanceId, raisedByActorId, created)
        {
            Completion = completion;
        }

        public override void DoTransactionAction(TransactionInstance instance)
        {
            instance.Completion = Completion;
        }
    }

    public class InitiatorAssigned : TransactionEvent
    {
        public int InitiatorId { get; set; }

        public InitiatorAssigned(int transactionInstanceId, int raisedByActorId, DateTime created, int initiatorId) 
            : base(TransactionEventType.InitiatorAssigned, transactionInstanceId, raisedByActorId, created)
        {
            this.InitiatorId = initiatorId;
        }

        public override void DoTransactionAction(TransactionInstance instance)
        {
            instance.InitiatorId = InitiatorId;
        }
    }
}