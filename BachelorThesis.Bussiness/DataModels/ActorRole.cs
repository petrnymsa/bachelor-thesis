using System.Threading;

namespace BachelorThesis.Bussiness.DataModels
{
    public class ActorRole
    {
        private static int nextId;

        public int Id { get; set; }
        public string Name { get; set; }

        public ActorRole()
        {
            
        }

        public ActorRole(string name)
        {
            Id = Interlocked.Increment(ref nextId);
            Name = name;
        }
    }
}