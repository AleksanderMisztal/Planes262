namespace GameServer.Matchmaking
{
    public readonly struct User
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
