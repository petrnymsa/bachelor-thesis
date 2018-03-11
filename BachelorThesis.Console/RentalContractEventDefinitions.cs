using System.Collections.Generic;
using BachelorThesis.Bussiness.DataModels;

namespace BachelorThesis.ConsoleTest
{
    public class RentalContractEventDefinitions : IProcessEventDefinitions
    {
        private static List<TransactionEventKind> events;

        public RentalContractEventDefinitions()
        {
            events = new List<TransactionEventKind>
            {
                new TransactionEventKind("Completion changed",
                    "Raised when completion of given transaction was changed.")
            };
        }

        public List<TransactionEventKind> GetDefinitions() => events;
        public TransactionEventKind FindById(int id) => events.Find(x => x.Id == id);
        public static List<TransactionEventKind> GetList() => events;

        public static TransactionEventKind Find(int id) => events.Find(x => x.Id == id);
    }
}