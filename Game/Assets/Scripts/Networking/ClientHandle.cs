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
            byte[] bytes = Serializer.Deserialize(byteArray);
            using (Packet packet = new Packet(bytes))
            {
                int packetType = packet.ReadInt();
                packetHandlers[packetType](packet);
            }
        }


        private static void Welcome(Packet packet)
        {
            EventHandlers.OnWelcome();
        }

        private static void GameJoined(Packet packet)
        {
            string opponentName = packet.ReadString();
            PlayerSide side = (PlayerSide)packet.ReadInt();
            Board board = packet.ReadBoard();

            GameState.Instance = new GameState(board);
            EventHandlers.OnGameJoined(opponentName, side, board);
        }

        private static void TroopSpawned(Packet packet)
        {
            List<Troop> troops = packet.ReadTroops();

            GameState.Instance.BeginNextRound(troops);
            EventHandlers.OnTroopsSpawned(troops);
        }

        private static void TroopMoved(Packet packet)
        {
            VectorTwo position = packet.ReadVector2Int();
            int direction = packet.ReadInt();
            List<BattleResult> battleResults = packet.ReadBattleResults();

            GameState.Instance.MoveTroop(position, direction, battleResults);
            EventHandlers.OnTroopMoved(position, direction, battleResults);
        }

        private static void GameEnded(Packet packet)
        {
            int redScore = packet.ReadInt();
            int blueScore = packet.ReadInt();

            EventHandlers.OnGameEnded(redScore, blueScore);
        }

        private static void MessageSent(Packet packet)
        {
            string message = packet.ReadString();

            EventHandlers.OnMessageSent(message);
        }

        private static void OpponentDisconnected(Packet packet)
        {
            EventHandlers.OnOpponentDisconnected();
            //UIManager.OpponentDisconnected();
        }
    }
}
