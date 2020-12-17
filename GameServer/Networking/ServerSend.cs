using System;
using GameDataStructures;
using GameDataStructures.Dtos;
using GameDataStructures.Messages.Server;

namespace GameServer.Networking
{
    public class ServerSend
    {
        private readonly Server server;

        public ServerSend(Server server)
        {
            this.server = server;
        }

        public void Welcome(int toClient, string[] gameTypes)
        {
            WelcomeMessage message = new WelcomeMessage(gameTypes);
            server.SendMessage(toClient, message);
        }
        
        public void GameJoined(int toClient, string opponentName, PlayerSide side, LevelDto levelDto, ClockInfo clockInfo)
        {
            GameJoinedMessage message = new GameJoinedMessage(opponentName, side, levelDto, clockInfo);
            server.SendMessage(toClient, message);
        }

        public void ChatSent(int toClient, string chatMessage)
        {
            ChatSentMessage message = new ChatSentMessage(chatMessage);
            server.SendMessage(toClient, message);
        }

        public void OpponentDisconnected(int toClient)
        {
            OpponentDisconnectedMessage message = new OpponentDisconnectedMessage();
            server.SendMessage(toClient, message);
        }

        public void LostOnTime(int redId, int blueId, PlayerSide loser)
        {
            Console.WriteLine($"Sending {loser} lost on time to clients {redId}, {blueId}");
            LostOnTimeMessage message = new LostOnTimeMessage(loser);
            server.SendMessage(redId, message);
            server.SendMessage(blueId, message);
        }

        
        public void TroopsSpawned(int redId, int blueId, TroopDto[] troops, TimeInfo timeInfo)
        {
            TroopsSpawnedMessage message = new TroopsSpawnedMessage(troops, timeInfo);
            server.SendMessage(redId, message);
            server.SendMessage(blueId, message);
        }

        public void TroopMoved(int redId, int blueId, TroopMovedMessage message)
        {
            server.SendMessage(redId, message);
            server.SendMessage(blueId, message);
        }

        public void GameEnded(int redId, int blueId, GameEndedMessage message)
        {
            server.SendMessage(redId, message);
            server.SendMessage(blueId, message);
        }
    }
}
