using System.Collections.Generic;
using GameDataStructures;
using GameJudge;

namespace GameServer.Matchmaking
{
    public class Game
    {
        public readonly User RedUser;
        public readonly User BlueUser;
        private readonly GameController controller;
        private readonly Clock clock;

        public Game(User redUser, User blueUser, GameController controller)
        {
            RedUser = redUser;
            BlueUser = blueUser;
            this.controller = controller;
            clock = new Clock(60, 5);
        }

        public void MakeMove(int client, VectorTwo position, int direction)
        {
            PlayerSide player = GetColor(client);
            clock.OnMoveMade();
            controller.ProcessMove(player, position, direction);
        }

        public TimeInfo ToggleActivePlayer()
        {
            return clock.ToggleActivePlayer();
        }

        private PlayerSide GetColor(int client)
        {
            if (client == RedUser.Id) return PlayerSide.Red;
            if (client == BlueUser.Id) return PlayerSide.Blue;

            throw new KeyNotFoundException("Get color didn't work");
        }
    }
}