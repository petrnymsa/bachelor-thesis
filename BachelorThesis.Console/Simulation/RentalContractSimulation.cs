using BachelorThesis.Bussiness.DataModels;

namespace BachelorThesis.ConsoleTest
{
    public class RentalContractSimulation : ProcessSimulation
    {
        private TransactionInstance rentalContracting;
        private TransactionInstance rentalPayment;
        private TransactionInstance carPickUp;
        private TransactionInstance carDropOff;
        private TransactionInstance penaltyPayment;

        public RentalContractSimulation(ProcessInstance processInstance)
        {
            this.ProcessInstance = processInstance;
            this.rentalContracting = processInstance.GetTransactionByIdentificator("T1");
            this.rentalPayment = processInstance.GetTransactionByIdentificator("T2");
            this.carPickUp = processInstance.GetTransactionByIdentificator("T3");
            this.carDropOff = processInstance.GetTransactionByIdentificator("T4");
            this.penaltyPayment = processInstance.GetTransactionByIdentificator("T5");
        }


        public override void Prepare()
        {
            //int ACTOR_ID = 0;
            //int eventId = RentalContractEventDefinitions.GetList().First().Id;
            //AddChunk(new SimulationChunk()
            //    .AddStep(SimulationStep.InitiatorAssigned(rentalContracting, ACTOR_ID,ACTOR_ID)));
            //AddChunk(new SimulationChunk()
            //    .AddStep(SimulationStep.ChangeTransactionCompletion(rentalContracting, TransactionCompletion.Requested,ACTOR_ID))
            //    .AddStep(SimulationStep.ChangeTransactionCompletion(rentalPayment,TransactionCompletion.Requested,ACTOR_ID)));

            //AddChunk(new SimulationChunk()
            //    .AddStep(SimulationStep.ChangeTransactionCompletion(rentalPayment, TransactionCompletion.Promised,ACTOR_ID))
            //    .AddStep(SimulationStep.ChangeTransactionCompletion(rentalContracting, TransactionCompletion.Promised,ACTOR_ID)));
        }
    }
}