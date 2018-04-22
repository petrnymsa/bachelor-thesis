using BachelorThesis.Business.DataModels;

namespace BachelorThesis.Business.Simulation
{
    public class SimulationCompletionChangedStep : SimulationStep
    {
        public SimulationCompletionChangedStep(TransactionEvent transactionEvent) : base(transactionEvent)
        {
        }

        public override TransactionEvent Simulate(ProcessInstance process)
        {
            var instance = process.GetTransactionById(Event.TransactionInstanceId);

            instance.Completion = Event.Completion;

            return Event;
        }
    }
}