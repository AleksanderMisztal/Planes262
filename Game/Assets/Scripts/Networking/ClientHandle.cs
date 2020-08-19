using System.Collections.Generic;
using Planes262.GameLogic;
using Planes262.GameLogic.Utils;
using Planes262.Networking.Packets;
using Planes262.UnityLayer;

namespace Planes262.Networking
{
    public static class ClientHandle
    {
        private delegate void PacketHandler(Packet packet);
        private static readonly Dictionary<int, PacketHandler> packetHandlers = new Dictionary<int, PacketHandler>
        {
            {(int) ServerPackets.Welcome, Welcome },
            {(int) ServerPackets.GameJoined, GameJoined },
            {(int) ServerPackets.TroopSpawned, TroopSpawned },
            {(int) ServerPackets.TroopMoved, TroopMoved },
            {(int) ServerPackets.GameEnded, GameEnded },
            {(int) ServerPackets.OpponentDisconnected, OpponentDisconnected },
            {(int) ServerPackets.MessageSent, MessageSent },
        };

        // TODO: Move string to byte conversion into Client class
        public static void HandlePacket(string byteArray)
        {
            var bytes = Serializer.Deserialize(byteArray);
            using (var packet = new Packet(bytes))
            {
                var packetType = packet.ReadInt();
                packetHandlers[packetType](packet);
            }
        }


        private static void Welcome(Packet packet)
        {
            EventHandlers.OnWelcome();
        }

        private static void GameJoined(Packet packet)
        {
            var opponentName = packet.ReadString();
            var side = (PlayerSide)packet.ReadInt();
            var board = packet.ReadBoard();

            GameState.Instance = new GameState(board);
            EventHandlers.OnGameJoined(opponentName, side, board);
        }

        private static void TroopSpawned(Packet packet)
        {
            var troops = packet.ReadTroops();

            GameState.Instance.BeginNextRound(troops);
            EventHandlers.OnTroopsSpawned(troops);
        }

        private static void TroopMoved(Packet packet)
        {
            var position = packet.ReadVector2Int();
            var direction = packet.ReadInt();
            var battleResults = packet.ReadBattleResults();

            GameState.Instance.MoveTroop(position, direction, battleResults);
            EventHandlers.OnTroopMoved(position, direction, battleResults);
        }

        private static void GameEnded(Packet packet)
        {
            var redScore = packet.ReadInt();
            var blueScore = packet.ReadInt();

            EventHandlers.OnGameEnded(redScore, blueScore);
        }

        private static void MessageSent(Packet packet)
        {
            var message = packet.ReadString();

            EventHandlers.OnMessageSent(message);
        }

        private static void OpponentDisconnected(Packet packet)
        {
            EventHandlers.OnOpponentDisconnected();
            //UIManager.OpponentDisconnected();
        }
    }
}
