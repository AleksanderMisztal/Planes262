namespace GameServer.Matchmaking
{
    public class User
    {
        public readonly int Id;
        public readonly string Name;

        public User(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
