﻿using BachelorThesis.Bussiness.DataModels;

namespace BachelorThesis.ConsoleTest
{
    public abstract class SimulationStep
    {
        public TransactionEvent Event { get; protected set; }

        protected SimulationStep(TransactionEvent transactionEvent)
        {
            Event = transactionEvent;
        }

        public abstract TransactionEvent Simulate(ProcessInstance process);
    }
}