using System;
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
        private GameController controller;

        public event Action<Game> GameEnded;

        public Game(User redUser, User blueUser, ServerSend sender)
        {
            this.redUser = redUser;
            this.blueUser = blueUser;
            this.sender = sender;
        }

        public void Initialize()
        {
            WaveProvider waveProvider = new WaveProvider(new List<Troop> {
                TroopFactory.Blue(1, 3),
                TroopFactory.Red(5, 3),
            }, new Dictionary<int, List<Troop>>());
            Board board = new Board(12, 7);
            controller = new GameController(waveProvider, board);
            
            Clock clock = new Clock(30, 5, side => sender.LostOnTime(redUser.id, blueUser.id, side));
            
            controller.TroopsSpawned += args => {
                TimeInfo timeInfo = clock.ToggleActivePlayer();
                sender.TroopsSpawned(redUser.id, blueUser.id, args, timeInfo);
            };
            controller.TroopMoved += args => sender.TroopMoved(redUser.id, blueUser.id, args);
            controller.GameEnded += args => {
                sender.GameEnded(redUser.id, blueUser.id, args);
                GameEnded?.Invoke(this);
            };

            ClockInfo clockInfo = clock.Initialize();
            sender.GameJoined(redUser.id, blueUser.name, PlayerSide.Red, board, waveProvider.initialTroops, clockInfo);
            sender.GameJoined(blueUser.id, redUser.name, PlayerSide.Blue, board, waveProvider.initialTroops, clockInfo);
        }
        
        public void MakeMove(int client, VectorTwo position, int direction)
        {
            PlayerSide player = client == redUser.id ? PlayerSide.Red : PlayerSide.Blue;
            controller.ProcessMove(player, position, direction);
        }
    }
}