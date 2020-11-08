using System;
using GameDataStructures;
using GameDataStructures.Messages.Server;
using GameJudge.GameEvents;

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
            WelcomeMessage message = new WelcomeMessage{gameTypes = gameTypes};
            server.SendMessage(toClient, message);
        }
        
        public void GameJoined(int toClient, string opponentName, PlayerSide side, Board board, TroopDto[] troops, ClockInfo clockInfo)
        {
            GameJoinedMessage message = new GameJoinedMessage
            {
                opponentName = opponentName,
                side = side,
                board = board,
                troops = troops,
                clockInfo = clockInfo,
            };

            server.SendMessage(toClient, message);
        }

        public void ChatSent(int toClient, string chatMessage)
        {
            ChatSentMessage message = new ChatSentMessage{message = chatMessage};
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
            LostOnTimeMessage message = new LostOnTimeMessage{loser = loser};
            server.SendMessage(redId, message);
            server.SendMessage(blueId, message);
        }

        
        public void TroopsSpawned(int redId, int blueId, TroopDto[] troops, TimeInfo timeInfo)
        {
            TroopsSpawnedMessage message = new TroopsSpawnedMessage {troops = troops, timeInfo = timeInfo};
            server.SendMessage(redId, message);
            server.SendMessage(blueId, message);
        }

        public void TroopMoved(int redId, int blueId, TroopMovedEventArgs args)
        {
            TroopMovedMessage message = new TroopMovedMessage
            {
                position = args.position,
                direction = args.direction,
                battleResults = args.battleResults.ToArray(),
                scoreInfo = args.score,
            };

            server.SendMessage(redId, message);
            server.SendMessage(blueId, message);
        }

        public void GameEnded(int redId, int blueId, GameEndedEventArgs args)
        {
            GameEndedMessage message = new GameEndedMessage{scoreInfo = args.score};

            server.SendMessage(redId, message);
            server.SendMessage(blueId, message);
        }
    }
}
