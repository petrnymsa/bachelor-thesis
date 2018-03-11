using System;
using System.Threading;

namespace BachelorThesis.Bussiness.DataModels
{

    public enum TransactionEventInternalName
    {
        Request,
        Promise,
        State,
        Accept,
        Reject,
        Stop,
        Quit,
        Execute,
        AssignExecutor,
        AssignInitiator
    }

    public class TransactionEventKind
    {
        private static int nextId;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public TransactionEventKind(string name, string description)
        {
            Id = Interlocked.Increment(ref nextId);
            Name = name;
            Description = description;
        }

    
    }

    //public class CompletionChangedEvent : TransactionEventKind
    //{
    //    public int InstanceId { get; }
    //    public TransactionCompletion OldCompletion { get; }
    //    public TransactionCompletion NewCompletion { get; }

    //    public CompletionChangedEvent(int instanceId, TransactionCompletion oldCompletion, TransactionCompletion newCompletion)
    //        :base("Completion changed")
    //    {
    //        this.InstanceId = instanceId;
    //        this.OldCompletion = oldCompletion;
    //        this.NewCompletion = newCompletion;
    //    }
    //}

    //public class ExecutorAssignedEvent : TransactionEventKind
    //{
    //    public ExecutorAssignedEvent(string name, TransactionEventInternalName internalName) : base(name)
    //    {
    //    }
    //}

    //public class InitiatorAssignedEvent : TransactionEventKind
    //{
    //    public InitiatorAssignedEvent(string name, TransactionEventInternalName internalName) : base(name)
    //    {
    //    }
    //}
}