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
}