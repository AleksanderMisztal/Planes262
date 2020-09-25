using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameDataStructures;
using GameJudge;
using GameJudge.Utils;
using GameJudge.WavesN;
using GameServer.Networking;

namespace GameServer.Matchmaking
{
    public class GameHandler
    {
        private readonly ServerSend sender;

        public GameHandler(ServerSend sender)
        {
            this.sender = sender;
        }

        private readonly Dictionary<int, Game> clientToGame = new Dictionary<int, Game>();

        private bool someoneWaiting;
        private User waitingUser;

        public async Task SendToGame(User newUser)
        {
            if (someoneWaiting)
            {
                someoneWaiting = false;
                Randomizer.RandomlyAssign(newUser, waitingUser, out User redUser, out User blueUser);
                await InitializeNewGame(redUser, blueUser);
            }
            else
            {
                someoneWaiting = true;
                waitingUser = newUser;
            }
        }

        private async Task InitializeNewGame(User playingRed, User playingBlue)
        {
            Waves waves = Waves.Test();
            Board board = Board.Test;
            GameController gc = new GameController(waves, board);
            Game game = new Game(playingRed, playingBlue, gc);

            clientToGame[playingRed.Id] = game;
            clientToGame[playingBlue.Id] = game;
            
            gc.TroopsSpawned += async args => {
                TimeInfo timeInfo = game.ToggleActivePlayer();
                await sender.TroopsSpawned(playingRed.Id, playingBlue.Id, args, timeInfo);
            };
            gc.TroopMoved += async args => await sender.TroopMoved(playingRed.Id, playingBlue.Id, args);
            gc.GameEnded += async args => await sender.GameEnded(playingRed.Id, playingBlue.Id, args);

            await sender.GameJoined(playingRed.Id, playingBlue.Name, PlayerSide.Red, board);
            await sender.GameJoined(playingBlue.Id, playingRed.Name, PlayerSide.Blue, board);
            
            gc.BeginGame();
        }

        public void MoveTroop(int client, VectorTwo position, int direction)
        {
            if (direction < -1 || direction > 1)
            {
                Console.WriteLine($"Client {client} sent a move with illegal direction!");
                return;
            }
            Game game = clientToGame[client];
            game.MakeMove(client, position, direction);
        }

        public async Task SendMessage(int client, string message)
        {
            int opponent = GetOpponent(client);
            await sender.MessageSent(opponent, message);
        }

        public async Task ClientDisconnected(int client)
        {
            if (someoneWaiting && client == waitingUser.Id)
                someoneWaiting = false;

            int opponent = GetOpponent(client);

            clientToGame.Remove(client);
            clientToGame.Remove(opponent);

            await sender.OpponentDisconnected(opponent);
        }

        private int GetOpponent(int client)
        {
            Game game = clientToGame[client];
            int opponentId = game.BlueUser.Id ^ game.RedUser.Id ^ client;
            return opponentId;
        }
    }
}