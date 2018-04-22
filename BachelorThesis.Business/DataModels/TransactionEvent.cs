using System;
using System.Runtime.Serialization;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BachelorThesis.Business.DataModels
{

    public enum TransactionEventType
    {
        CompletionChanged,
        InitiatorAssigned
    }
    [DataContract]
    public class TransactionEvent
    {
        private static int nextId;

        [JsonProperty]
        public int Id { get; set; }
        [JsonProperty]
        public TransactionEventType EventType { get; set; }
        //  public int ProcessInstanceId { get; set; } //TODO parsers must recognize it
        [JsonProperty]
        public int TransactionInstanceId { get; set; }
        [JsonProperty]
        public int TransactionKindId { get; set; }
        [JsonProperty]
        public int RaisedByActorId { get; set; }
        [JsonProperty]
        public DateTime Created { get; set; }

        [JsonProperty]
        public TransactionCompletion Completion { get; }

        public TransactionEvent(TransactionEventType eventType, int transactionInstanceId, int transactionKindId, int raisedByActorId, DateTime created, TransactionCompletion completion)
        {
            Id = Interlocked.Increment(ref nextId);
            EventType = eventType;
            TransactionInstanceId = transactionInstanceId;
            TransactionKindId = transactionKindId;
            RaisedByActorId = raisedByActorId;
            Created = created;
        }

     //   public abstract void DoTransactionAction(TransactionInstance instance);
    }

    //public class CompletionChangedTransactionEvent : TransactionEvent
    //{
        

    //    public CompletionChangedTransactionEvent(int transactionInstanceId, int transactionKindId, int raisedByActorId, DateTime created, TransactionCompletion completion)
    //        : base(TransactionEventType.CompletionChanged, transactionInstanceId, transactionKindId, raisedByActorId, created)
    //    {
    //        Completion = completion;
    //    }

    //    public override void DoTransactionAction(TransactionInstance instance)
    //    {
    //        instance.Completion = Completion;
    //    }
    //}

    //public class InitiatorAssigned : TransactionEvent
    //{
    //    public int InitiatorId { get; set; }

    //    public InitiatorAssigned(int transactionInstanceId, int raisedByActorId, DateTime created, int initiatorId) 
    //        : base(TransactionEventType.InitiatorAssigned, transactionInstanceId, raisedByActorId, created)
    //    {
    //        this.InitiatorId = initiatorId;
    //    }

    //    public override void DoTransactionAction(TransactionInstance instance)
    //    {
    //        instance.InitiatorId = InitiatorId;
    //    }
    //}
}