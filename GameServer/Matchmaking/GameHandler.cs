using System.Collections.Generic;
using System.Threading.Tasks;
using GameDataStructures;
using GameDataStructures.Positioning;
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
                await InitializeNewGame(newUser, waitingUser);
            }
            else
            {
                someoneWaiting = true;
                waitingUser = newUser;
            }
        }

        private async Task InitializeNewGame(User u1, User u2)
        {
            Randomizer.RandomlyAssign(u1, u2, out User redUser, out User blueUser);
            Game game = new Game(redUser, blueUser, sender);

            clientToGame[redUser.Id] = game;
            clientToGame[blueUser.Id] = game;

            await game.Initialize();
        }

        public void MoveTroop(int client, VectorTwo position, int direction)
        {
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
            return game.blueUser.Id ^ game.redUser.Id ^ client;
        }
    }
}