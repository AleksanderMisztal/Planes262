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

        public Game(User redUser, User blueUser, GameController controller)
        {
            RedUser = redUser;
            BlueUser = blueUser;
            this.controller = controller;
        }

        public void MakeMove(int client, VectorTwo position, int direction)
        {
            PlayerSide player = GetColor(client);
            controller.ProcessMove(player, position, direction);
        }

        private PlayerSide GetColor(int client)
        {
            if (client == RedUser.Id) return PlayerSide.Red;
            if (client == BlueUser.Id) return PlayerSide.Blue;

            throw new KeyNotFoundException();
        }
    }
}