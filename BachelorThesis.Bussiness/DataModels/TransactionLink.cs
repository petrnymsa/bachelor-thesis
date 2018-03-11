using System.Threading;

namespace BachelorThesis.Bussiness.DataModels
{

    public enum TransactionLinkCardinality
    {
        One,
        OneToMany,
        ZeroToMany,
        ZeroToOne,
        Interval
    }

    public class CardinalityInterval
    {
        public int Min { get; set; }

        public int Max { get; set; }

        public CardinalityInterval(int min, int max)
        {
            this.Min = min;
            this.Max = max;
        }
    }

    public class TransactionLink
    {
        private static int nextId;

        public int Id { get; set; }
        public int SourceTransactionKindId { get; set; }
        public int DestinationTransactionKindId { get; set; }
        public TransactionCompletion SourceCompletion { get; set; }
        public TransactionCompletion DestinationCompletion { get; set; }

        public TransactionLinkType Type { get; set; }

        public TransactionLinkCardinality SourceCardinality { get; set; }

        public TransactionLinkCardinality DestinationCardinality { get; set; }

        public CardinalityInterval SourceCardinalityInterval { get; set; }
        public CardinalityInterval DestinationCardinalityInterval { get; set; }

        public TransactionLink()
        {
            Id = Interlocked.Increment(ref nextId);
        }
    }
}