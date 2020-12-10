using System;
using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge;
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

        public void Initialize(GameConfig config)
        {
            controller = new GameController(new WaveProvider(config.levelDto.troopDtos), config.levelDto.board.Get());
            
            Clock clock = new Clock(config.time, config.increment, side =>
            {
                sender.LostOnTime(redUser.id, blueUser.id, side);
                GameEnded?.Invoke(this);
            });
            
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
            sender.GameJoined(redUser.id, blueUser.name, PlayerSide.Red, config.levelDto, clockInfo);
            sender.GameJoined(blueUser.id, redUser.name, PlayerSide.Blue, config.levelDto, clockInfo);
        }

        public void EndRound(int client)
        {
            PlayerSide player = client == redUser.id ? PlayerSide.Red : PlayerSide.Blue;
            controller.EndRound(player);
        }
        
        public void MakeMove(int client, VectorTwo position, int direction)
        {
            PlayerSide player = client == redUser.id ? PlayerSide.Red : PlayerSide.Blue;
            controller.ProcessMove(player, position, direction);
        }
    }
}