using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge;
using GameJudge.Troops;
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

            waveProvider = new WaveProvider(new List<Troop> {
                TroopFactory.Blue(1, 3),
                TroopFactory.Red(5, 3),
            }, new Dictionary<int, List<Troop>>());
            board = new Board(12, 7);
            controller = new GameController(waveProvider, board);
            
            clock = new Clock(30, 5, side => sender.LostOnTime(redUser.id, blueUser.id, side));
        }

        public void Initialize()
        {
            controller.TroopsSpawned += args => {
                TimeInfo timeInfo = clock.ToggleActivePlayer();
                sender.TroopsSpawned(redUser.id, blueUser.id, args, timeInfo);
            };
            controller.TroopMoved += args => sender.TroopMoved(redUser.id, blueUser.id, args);
            controller.GameEnded += args => sender.GameEnded(redUser.id, blueUser.id, args);

            ClockInfo clockInfo = clock.Initialize();
            sender.GameJoined(redUser.id, blueUser.name, PlayerSide.Red, board, waveProvider.initialTroops, clockInfo);
            sender.GameJoined(blueUser.id, redUser.name, PlayerSide.Blue, board, waveProvider.initialTroops, clockInfo);
        }
        
        public void MakeMove(int client, VectorTwo position, int direction)
        {
            PlayerSide player = GetColor(client);
            controller.ProcessMove(player, position, direction);
        }

        private PlayerSide GetColor(int client)
        {
            if (client == redUser.id) return PlayerSide.Red;
            if (client == blueUser.id) return PlayerSide.Blue;

            throw new KeyNotFoundException("Get color didn't work");
        }
    }
}