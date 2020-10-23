using System.Collections.Generic;
using System.Threading.Tasks;
using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge;
using GameJudge.Waves;
using GameServer.Networking;

namespace GameServer.Matchmaking
{
    public class Game
    {
        public readonly User redUser;
        public readonly User blueUser;
        private readonly ServerSend sender;
        private readonly GameController controller;
        private readonly Clock clock;
        private readonly Board board;
        private readonly WaveProvider waveProvider;

        public Game(User redUser, User blueUser, ServerSend sender)
        {
            this.redUser = redUser;
            this.blueUser = blueUser;
            this.sender = sender;

            waveProvider = WaveProvider.Test();
            board = Board.test;
            controller = new GameController(waveProvider, board);
            
            clock = new Clock(10, 5, async side => await sender.LostOnTime(redUser.Id, blueUser.Id, side));
        }

        public async Task Initialize()
        {
            controller.TroopsSpawned += async args => {
                TimeInfo timeInfo = clock.ToggleActivePlayer();
                await sender.TroopsSpawned(redUser.Id, blueUser.Id, args, timeInfo);
            };
            controller.TroopMoved += async args => await sender.TroopMoved(redUser.Id, blueUser.Id, args);
            controller.GameEnded += async args => await sender.GameEnded(redUser.Id, blueUser.Id, args);

            ClockInfo clockInfo = clock.Initialize();
            await sender.GameJoined(redUser.Id, blueUser.Name, PlayerSide.Red, board, waveProvider.initialTroops, clockInfo);
            await sender.GameJoined(blueUser.Id, redUser.Name, PlayerSide.Blue, board, waveProvider.initialTroops, clockInfo);
        }
        
        public void MakeMove(int client, VectorTwo position, int direction)
        {
            PlayerSide player = GetColor(client);
            controller.ProcessMove(player, position, direction);
        }

        private PlayerSide GetColor(int client)
        {
            if (client == redUser.Id) return PlayerSide.Red;
            if (client == blueUser.Id) return PlayerSide.Blue;

            throw new KeyNotFoundException("Get color didn't work");
        }
    }
}