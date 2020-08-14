using System.Collections.Generic;
using Assets.Scripts.UnityStuff;
using GameServer.GameLogic;
using GameServer.Utils;
using Scripts.Utils;

namespace Scripts.Networking
{
    public static class ClientHandle
    {
        private delegate void PacketHandler(Packet _packet);
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


        public static void Welcome(Packet packet)
        {
            GameInstance.OnWelcome();
        }

        public static void GameJoined(Packet packet)
        {
            string opponentName = packet.ReadString();
            PlayerSide side = (PlayerSide)packet.ReadInt();
            Board board = packet.ReadBoard();

            GameInstance.OnGameJoined(opponentName, side, board);
        }

        public static void TroopSpawned(Packet packet)
        {
            List<Troop> troops = packet.ReadTroops();

            GameInstance.OnTroopsSpawned(troops);
        }

        public static void TroopMoved(Packet packet)
        {
            Vector2Int position = packet.ReadVector2Int();
            int direction = packet.ReadInt();
            List<BattleResult> battleResults = packet.ReadBattleResults();

            GameInstance.OnTroopMoved(position, direction, battleResults);
        }

        public static void GameEnded(Packet packet)
        {
            int redScore = packet.ReadInt();
            int blueScore = packet.ReadInt();

            GameInstance.OnGameEnded(redScore, blueScore);
        }

        public static void MessageSent(Packet packet)
        {
            string message = packet.ReadString();

            GameInstance.OnMessageSent(message);
        }

        public static void OpponentDisconnected(Packet packet)
        {
            GameInstance.OnOpponentDisconnected();
            //UIManager.OpponentDisconnected();
        }
    }
}
