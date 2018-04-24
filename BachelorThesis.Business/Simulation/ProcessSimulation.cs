using System.Collections.Generic;
using BachelorThesis.Business.DataModels;

namespace BachelorThesis.Business.Simulation
{
    public abstract class ProcessSimulation
    {
        public ProcessInstance ProcessInstance { get; protected set; }
        public ProcessKind ProcessKind { get; set; }
        public List<Actor> Actors { get; protected set; }

        private int currentChunk = 0;
        private List<SimulationChunk> chunks;

        public bool CanContinue => currentChunk < chunks.Count;
        public string Name { get; protected set; }

        protected ProcessSimulation()
        {
            chunks = new List<SimulationChunk>();
            Actors = new List<Actor>();
        }

        public abstract void Prepare();

        public List<TransactionEvent> SimulateNextChunk()
        {
            if (currentChunk >= chunks.Count) return null;

            var result = chunks[currentChunk++].Simulate(ProcessInstance);
            return result;

        }

        protected ProcessSimulation AddChunk(SimulationChunk chunk)
        {
            chunks.Add(chunk);
            return this;
        }

        protected void ResetChunks()
        {
            chunks.Clear();
            currentChunk = 0;
        }


        public Actor FindActorById(int raisedByActorId) => Actors.Find(x => x.Id == raisedByActorId);

        public void Reset()
        {
            ResetChunks();
            Actors.Clear();
            Prepare();
        }
    }
}