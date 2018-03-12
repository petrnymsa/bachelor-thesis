using BachelorThesis.Bussiness.DataModels;

namespace BachelorThesis.Bussiness.Simulation
{
    public class SimulationCompletionChangedStep : SimulationStep
    {
        public SimulationCompletionChangedStep(CompletionChangedTransactionEvent transactionEvent) : base(transactionEvent)
        {
        }

        public override TransactionEvent Simulate(ProcessInstance process)
        {
            var instance = process.GetTransactionById(Event.TransactionInstanceId);

            instance.Completion = ((CompletionChangedTransactionEvent) Event).Completion;

            return Event;
        }
    }
}