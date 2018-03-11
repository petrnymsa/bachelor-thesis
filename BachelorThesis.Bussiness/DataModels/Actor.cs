using System.Threading;

namespace BachelorThesis.Bussiness.DataModels
{
    public class ActorKind
    {
        private static int nextId;

        public int Id { get; set; }
        public string Name { get; set; }


    }

    public class Actor
    {
        private static int nextId;

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Actor()
        {
            
        }

        public Actor(string firstName, string lastName)
        {
            Id = Interlocked.Increment(ref nextId);
            FirstName = firstName;
            LastName = lastName;
        }
    }
}