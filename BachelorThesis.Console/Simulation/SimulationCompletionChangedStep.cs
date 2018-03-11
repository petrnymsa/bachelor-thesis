using BachelorThesis.Bussiness.DataModels;

namespace BachelorThesis.ConsoleTest
{
    public class SimulationCompletionChangedStep : SimulationStep
    {
        public SimulationCompletionChangedStep(CompletionChangedTransactionEvent transactionEvent) : base(transactionEvent)
        {
        }

        public override TransactionEvent Simulate(ProcessInstance process)
        {
            var instance = process.GetTransactionById(Event.TransactionInstanceId);

            instance.CompletionType = ((CompletionChangedTransactionEvent) Event).NewCompletion;

            return Event;
        }
    }
}