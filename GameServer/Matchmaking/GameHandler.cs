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

        private readonly Dictionary<int, Game> games = new Dictionary<int, Game>();
        private readonly Dictionary<string, User> waitingUsers = new Dictionary<string, User>();
        private readonly Dictionary<int, string> users = new Dictionary<int, string>();

        public GameHandler(ServerSend sender)
        {
            this.sender = sender;
        }

        public void SendToGame(User newUser, string gameType)
        {
            if (users.TryGetValue(newUser.id, out _)) return;
            users[newUser.id] = gameType;
            
            Console.WriteLine($"Sending client {newUser.id} with name {newUser.name} to game of type <{gameType}>.");
            if (waitingUsers.TryGetValue(gameType, out User waitingUser))
            {
                waitingUsers.Remove(gameType);
                InitializeNewGame(newUser, waitingUser, GameConfig.configs[gameType]);
            }
            else waitingUsers[gameType] = newUser;
        }

        private void InitializeNewGame(User u1, User u2, GameConfig config)
        {
            Randomizer.RandomlyAssign(u1, u2, out User redUser, out User blueUser);
            Game game = new Game(redUser, blueUser, sender);
            game.GameEnded += HandleGameEnd;

            games[redUser.id] = game;
            games[blueUser.id] = game;

            game.Initialize(config);
        }

        private void HandleGameEnd(Game game)
        {
            games.Remove(game.redUser.id);
            games.Remove(game.blueUser.id);
            users.Remove(game.redUser.id);
            users.Remove(game.blueUser.id);
        }

        public void MoveTroop(int client, VectorTwo position, int direction)
        {
            if (!games.TryGetValue(client, out Game game)) return;
            game.MakeMove(client, position, direction);
        }

        public void SendMessage(int client, string message)
        {
            if (!games.TryGetValue(client, out Game game)) return;
            int opponent = game.blueUser.id ^ game.redUser.id ^ client;
            sender.ChatSent(opponent, message);
        }

        public void ClientDisconnected(int client)
        {
            if (!users.TryGetValue(client, out string gameType)) return;
            if (waitingUsers.TryGetValue(gameType, out User user) && user.id == client)
            {
                waitingUsers.Remove(gameType);
                return;
            }
            if (!games.TryGetValue(client, out Game game)) return;
            int opponent = game.blueUser.id ^ game.redUser.id ^ client;

            games.Remove(client);
            games.Remove(opponent);

            sender.OpponentDisconnected(opponent);
        }
    }
}