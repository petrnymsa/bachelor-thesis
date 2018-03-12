using System.Threading;

namespace BachelorThesis.Bussiness.DataModels
{
    public class ActorKind
    {
        private static int nextId;

        public int Id { get; set; }
        public string Name { get; set; }

        public ActorKind(string name)
        {
            Id = Interlocked.Increment(ref nextId);
            Name = name;
        }
    }

    public class Actor
    {
        private static int nextId;

        public int Id { get; set; }
        public int ActorKindId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Fullname => $"{FirstName} {LastName}";

        public Actor()
        {
            
        }

        public Actor(int actorKindId, string firstName, string lastName)
        {
            Id = Interlocked.Increment(ref nextId);
            ActorKindId = actorKindId;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}