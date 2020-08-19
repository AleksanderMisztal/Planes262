#if UNITY_EDITOR || !UNITY_WEBGL
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Planes262.Networking
{
    public class ThreadManager : MonoBehaviour
    {
        private static readonly List<Action> executeOnMainThread = new List<Action>();
        private static readonly List<Action> executeCopiedOnMainThread = new List<Action>();
        private static bool actionToExecuteOnMainThread = false;

        private void Update()
        {
            UpdateMain();
        }

        public static void ExecuteOnMainThread(Action action)
        {
            if (action == null) return;
            lock (executeOnMainThread)
            {
                executeOnMainThread.Add(action);
                actionToExecuteOnMainThread = true;
            }
        }

        private static void UpdateMain()
        {
            if (!actionToExecuteOnMainThread) return;
            executeCopiedOnMainThread.Clear();
            lock (executeOnMainThread)
            {
                executeCopiedOnMainThread.AddRange(executeOnMainThread);
                executeOnMainThread.Clear();
                actionToExecuteOnMainThread = false;
            }

            foreach (var action in executeCopiedOnMainThread) action();
        }
    }
}
#endif