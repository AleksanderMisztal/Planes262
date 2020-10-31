using System;
using System.Collections.Generic;
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

        public void SendToGame(User newUser)
        {
            if (newUser.id == waitingUser.id) return;
            Console.WriteLine($"Sending client {newUser.id} with name {newUser.name} to game");
            if (someoneWaiting)
            {
                someoneWaiting = false;
                InitializeNewGame(newUser, waitingUser);
            }
            else
            {
                someoneWaiting = true;
                waitingUser = newUser;
            }
        }

        private void InitializeNewGame(User u1, User u2)
        {
            Randomizer.RandomlyAssign(u1, u2, out User redUser, out User blueUser);
            Game game = new Game(redUser, blueUser, sender);
            game.GameEnded += HandleGameEnd;

            clientToGame[redUser.id] = game;
            clientToGame[blueUser.id] = game;

            game.Initialize();
        }

        private void HandleGameEnd(Game game)
        {
            clientToGame.Remove(game.redUser.id);
            clientToGame.Remove(game.blueUser.id);
        }

        public void MoveTroop(int client, VectorTwo position, int direction)
        {
            if (!clientToGame.TryGetValue(client, out Game game)) return;
            game.MakeMove(client, position, direction);
        }

        public void SendMessage(int client, string message)
        {
            if (!clientToGame.TryGetValue(client, out Game game)) return;
            int opponent = game.blueUser.id ^ game.redUser.id ^ client;
            sender.MessageSent(opponent, message);
        }

        public void ClientDisconnected(int client)
        {
            if (someoneWaiting && client == waitingUser.id)
            {
                someoneWaiting = false;
                return;
            }
            if (!clientToGame.TryGetValue(client, out _)) return;
            
            int opponent = GetOpponent(client);

            clientToGame.Remove(client);
            clientToGame.Remove(opponent);

            sender.OpponentDisconnected(opponent);
        }

        private int GetOpponent(int client)
        {
            Game game = clientToGame[client];
            return game.blueUser.id ^ game.redUser.id ^ client;
        }
    }
}