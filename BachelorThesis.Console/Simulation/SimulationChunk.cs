using System.Collections.Generic;
using BachelorThesis.Bussiness.DataModels;

namespace BachelorThesis.ConsoleTest
{
    public class SimulationChunk
    {
        private readonly List<SimulationStep> steps;

        public SimulationChunk()
        {
            this.steps = new List<SimulationStep>();
        }

        public SimulationChunk AddStep(TransactionEvent transactionEvent)
        {
            if(transactionEvent is CompletionChangedTransactionEvent completionChangedTransactionEvent)
                steps.Add(new SimulationCompletionChangedStep(completionChangedTransactionEvent));

            return this;
        }

        public List<TransactionEvent> Simulate(ProcessInstance process)
        {
            var events = new List<TransactionEvent>();

            foreach (var step in steps)
                events.Add(step.Simulate(process));

            return events;
        }
    }
}