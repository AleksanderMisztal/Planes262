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
        public static void OnConnect()
        {
            GameObject.FindObjectOfType<Activator>().GetComponent<SpriteRenderer>().color = Color.blue;
        }

        public static void OnTroopsSpawned(List<SpawnTemplate> templates)
        {
            GameController.instance.StartNextRound(templates);
        }

        public static void OnTroopMoved(Vector2Int position, int direction, List<BattleResult> battleResults)
        {
            GameController.instance.OnTroopMoved(position, direction, battleResults);
        }
    }
}
