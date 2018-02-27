using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace BachelorThesis.ConsoleTest
{
    public class Actor
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public enum TransactionState
    {
        None = 0,
        Requested,
        Promised,
        Stated,
        Accepted,
        Quitted,
        Stopped,
        Rejected
    }

    public class Transaction
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public DateTime RequestedTime { get; set; }
        public DateTime OptimisticEndTime { get; set; }
        public DateTime NormalEndTime { get; set; }
        public DateTime PessimisticEndTime { get; set; }
        public DateTime ExpectedEndTime => new DateTime((OptimisticEndTime.Ticks + 4 * NormalEndTime.Ticks + PessimisticEndTime.Ticks) / 6);
        public float Completion { get; set; }
        public TransactionState State { get; set; } = TransactionState.None;

        public List<TransactionEvent> Events { get; set; }

        public List<Transaction> Child { get; set; }
        public Transaction ParentTransaction { get; set; }

        public Actor Initiator { get; set; }
        public Actor Executor { get; set; }
        ////public double ExpectedTimeEstimate
        ////{
        ////    get
        ////    {
        ////        var o = GetNumberOfDaysBetween(RequestedTime, OptimisticEndTime);
        ////        var n = GetNumberOfDaysBetween(RequestedTime, NormalEndTime);
        ////        var p = GetNumberOfDaysBetween(RequestedTime, PessimisticEndTime);
        ////        Console.WriteLine(o);
        ////        Console.WriteLine(n);
        ////        Console.WriteLine(p);
        ////        return (o + 4 * n + p) / 6d;
        ////    }
        ////}

        //private static int GetNumberOfDaysBetween(DateTime a, DateTime b) => (b - a).Days;
        public Transaction() { }
        public Transaction(Guid id, string label, DateTime requestedTime = default(DateTime), DateTime optimisticEndTime = default(DateTime), DateTime normalEndTime = default(DateTime), DateTime pessimisticEndTime = default(DateTime), float completion = 0, List<TransactionEvent> events = null, Actor initiator = null, Actor executor = null)
        {
            Id = id;
            Label = label;
            RequestedTime = requestedTime;
            OptimisticEndTime = optimisticEndTime;
            NormalEndTime = normalEndTime;
            PessimisticEndTime = pessimisticEndTime;
            Completion = completion;
            Events = events;
            Initiator = initiator;
            Executor = executor;
        }
    }

    public class TransactionEvent
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public TransactionState TransactionState { get; set; }
        public DateTime CreatedTime { get; set; }

        public TransactionEvent(Guid id, string label, TransactionState transactionState, DateTime createdTime)
        {
            Id = id;
            Label = label;
            TransactionState = transactionState;
            CreatedTime = createdTime;
        }
    }

    public class Process
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ExpectedEndTime { get; set; }
        public float Completion { get; set; }

        public List<Transaction> Transactions { get; set; }
    }


    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var date = DateTime.Now;
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                Label = "Car pick up",
                RequestedTime = date,
                OptimisticEndTime = date.AddDays(2),
                NormalEndTime = date.AddDays(4),
                PessimisticEndTime = date.AddDays(6),
                Completion = 0.25f,
                State = TransactionState.Requested,
                Initiator = new Actor() {Id = Guid.NewGuid(), Name = "Alice"},
                Executor = new Actor() {Id = Guid.NewGuid(), Name = "Bob"},
                Events = new List<TransactionEvent>() { new TransactionEvent(Guid.NewGuid(), "Requested", TransactionState.Requested, date)},
            };


            var process = new Process()
            {
                Id = Guid.NewGuid(),
                Completion = 0.25f,
                ExpectedEndTime = date.AddDays(7),
                Name = "Car rent",
                StartTime = date,
                Transactions = new List<Transaction>() { transaction}
            };

            var json = JsonConvert.SerializeObject(process);
            Clipboard.SetText(json);
            Console.WriteLine(json);
            Console.WriteLine("---- END ---");
            Console.ReadKey();
        }
    }
}
