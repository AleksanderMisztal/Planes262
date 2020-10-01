using System.Collections.Generic;
using System.Threading.Tasks;
using GameDataStructures;
using GameJudge;
using GameJudge.WavesN;
using GameServer.Networking;

namespace GameServer.Matchmaking
{
    public class Game
    {
        public readonly User RedUser;
        public readonly User BlueUser;
        private readonly ServerSend sender;
        private readonly GameController controller;
        private readonly Clock clock;
        private readonly Board board;

        public Game(User redUser, User blueUser, ServerSend sender)
        {
            RedUser = redUser;
            BlueUser = blueUser;
            this.sender = sender;

            Waves waves = Waves.Test();
            board = Board.Test;
            controller = new GameController(waves, board);
            
            clock = new Clock(10, 5, async side => await sender.LostOnTime(redUser.Id, blueUser.Id, side));
        }

        public async Task Initialize()
        {
            controller.TroopsSpawned += async args => {
                TimeInfo timeInfo = clock.ToggleActivePlayer();
                await sender.TroopsSpawned(RedUser.Id, BlueUser.Id, args, timeInfo);
            };
            controller.TroopMoved += async args => await sender.TroopMoved(RedUser.Id, BlueUser.Id, args);
            controller.GameEnded += async args => await sender.GameEnded(RedUser.Id, BlueUser.Id, args);

            ClockInfo clockInfo = clock.Initialize();
            await sender.GameJoined(RedUser.Id, BlueUser.Name, PlayerSide.Red, board, clockInfo);
            await sender.GameJoined(BlueUser.Id, RedUser.Name, PlayerSide.Blue, board, clockInfo);

            controller.BeginGame();
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

            throw new KeyNotFoundException("Get color didn't work");
        }
    }
}