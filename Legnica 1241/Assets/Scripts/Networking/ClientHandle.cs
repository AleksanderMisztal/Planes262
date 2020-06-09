using Scripts.GameLogic;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Networking
{
    public class ClientHandle
    {
        public static void Welcome(Packet packet)
        {
            string message = packet.ReadString();
            int myId = packet.ReadInt();

            Debug.Log($"Received a message: {message}");
            NetworkingHub.OnConnect();
            Client.instance.myId = myId;
            ClientSend.WelcomeReceived();
            
        }

        public static void TroopSpawned(Packet packet)
        {
            Debug.Log($"Received Troop spawned!");

            int length = packet.ReadInt();

            List<SpawnTemplate> templates = new List<SpawnTemplate>();

            for (int i = 0; i < length; i++)
            {
                SpawnTemplate template = packet.ReadSpawnTemplate();

                templates.Add(template);

                Debug.Log($"Template: {template}");
            }
            NetworkingHub.OnTroopsSpawned(templates);
        }

        public static void TroopMoved(Packet packet)
        {
            Vector2Int position = packet.ReadVector2Int();
            int direction = packet.ReadInt();

            int length = packet.ReadInt();
            List<BattleResult> battleResults = new List<BattleResult>();
            for (int i = 0; i < length; i++)
            {
                battleResults.Add(packet.ReadBattleResult());
            }

            NetworkingHub.OnTroopMoved(position, direction, battleResults);
        }
    }
}
