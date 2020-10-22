using System;
using GameDataStructures;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Planes262.UnityLayer
{
    public static class TransitionManager
    {
        public static Board board;
        public static bool isLocal = false;
        public static string gameEndedMessage;
        public static string opponentName;
        public static PlayerSide side;
        public static ClockInfo clockInfo;

        static TransitionManager()
        {
            SceneManager.activeSceneChanged += (prev, next) => Debug.Log(prev.name + " => " + next.name);
        }

        public static event Action<string> OnBoardLoaded;

        public static void TransitionToBoard()
        {
            
        }
    }
}