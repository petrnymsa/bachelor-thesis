using System.Collections.Generic;
using BachelorThesis.Bussiness.DataModels;

namespace BachelorThesis.ConsoleTest
{
    public class RentalContractProcessDefinition : IProcessDefinition
    {
        private readonly ActorKind actorRenter;
        private readonly ActorKind actorCarIssuer;
        private readonly ActorKind actorDriver;
        private readonly ActorKind actorRentalContracter;

        public RentalContractProcessDefinition()
        {
            actorRenter = new ActorKind("Renter");
            actorDriver = new ActorKind("Driver");
            actorCarIssuer = new ActorKind("Car issuer");
            actorRentalContracter = new ActorKind("Rental contracter");
        }

        private List<TransactionKind> GetTransactionDefinitions(ProcessKind process)
        {
            var t1 = new TransactionKind("T1", "Rental contracting", process.Id, actorRenter.Id, actorRentalContracter.Id);
            var t2 = new TransactionKind("T2", "Rental payment", process.Id, actorRentalContracter.Id, actorRenter.Id);
            var t3 = new TransactionKind("T3", "Car pick up", process.Id, actorDriver.Id, actorCarIssuer.Id);
            var t4 = new TransactionKind("T4", "Car drop off", process.Id, actorCarIssuer.Id, actorDriver.Id);
            var t5 = new TransactionKind("T5", "Penalty payment", process.Id, actorCarIssuer.Id, actorDriver.Id);

            t1.AddChild(t2);
            t3.AddChild(t4);
            t3.AddChild(t5);

            process.AddTransactionLink(t1,t2,TransactionCompletion.Requested,TransactionCompletion.Promised,TransactionLinkType.Response);
            process.AddTransactionLink(t2,t1, TransactionCompletion.Promised, TransactionCompletion.Promised, TransactionLinkType.Waiting);
            process.AddTransactionLink(t2,t1, TransactionCompletion.Accepted, TransactionCompletion.Executed,TransactionLinkType.Waiting);

            process.AddTransactionLink(t3, t4, TransactionCompletion.Promised, TransactionCompletion.Requested, TransactionLinkType.Response);
            process.AddTransactionLink(t4, t3, TransactionCompletion.Promised, TransactionCompletion.Executed, TransactionLinkType.Waiting);

            var linkT4T5Reject = new TransactionLink()
            {
                SourceTransactionKindId = t4.Id,
                DestinationTransactionKindId = t5.Id,
                SourceCompletion = TransactionCompletion.Rejected,
                DestinationCompletion = TransactionCompletion.Requested,
                SourceCardinality = TransactionLinkCardinality.ZeroToOne,
                Type = TransactionLinkType.Response
            };
            process.AddTransactionLink(linkT4T5Reject);

            var linkT5T4Ac = new TransactionLink()
            {
                SourceTransactionKindId = t5.Id,
                DestinationTransactionKindId = t4.Id,
                SourceCompletion = TransactionCompletion.Accepted,
                DestinationCompletion = TransactionCompletion.Accepted,
                DestinationCardinality = TransactionLinkCardinality.ZeroToOne,
                Type = TransactionLinkType.Waiting
            };
            process.AddTransactionLink(linkT5T4Ac);


            return new List<TransactionKind>{t1,t3};
        }

        public ProcessKind GetDefinition()
        {
            var process = new ProcessKind("Rental contract");

            var transactionsTreeRoots = GetTransactionDefinitions(process);

            transactionsTreeRoots.ForEach(root => process.AddTransaction(root));

            return process;
        }

        public List<ActorKind> GetActorKinds() => new  List<ActorKind>() { actorRenter, actorDriver, actorRentalContracter, actorCarIssuer };
    }
}