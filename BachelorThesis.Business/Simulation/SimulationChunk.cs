using System.Collections.Generic;
using System.Linq;
using BachelorThesis.Business.DataModels;

namespace BachelorThesis.Business.Simulation
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
//            if(transactionEvent is CompletionChangedTransactionEvent completionChangedTransactionEvent)
//                steps.Add(new SimulationCompletionChangedStep(completionChangedTransactionEvent));
            steps.Add(new SimulationCompletionChangedStep(transactionEvent));
            return this;
        }

        public List<TransactionEvent> Simulate(ProcessInstance process)
        {
            var events = new List<TransactionEvent>();

            foreach (var step in steps)
                events.Add(step.Simulate(process));

            return events;
        }

        public List<TransactionEvent> GetEvents() => steps.Select(x => x.Event).ToList();
    }
}