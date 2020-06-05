using UnityEngine;
using Scripts.Networking;
using System.Collections.Generic;

namespace Scripts.GameLogic
{
    class NetworkingHub
    {
        // Game -> Server
        public static void SendTroopMove(Vector2Int position, int direction)
        {
            ClientSend.MoveTroop(position, direction);
        }


        // Server -> Game
        public static void OnTroopsSpawned(List<SpawnTemplate> templates)
        {
            GameController.instance.AddWave(templates);
            GameController.instance.StartNextRound();
        }
    }
}
