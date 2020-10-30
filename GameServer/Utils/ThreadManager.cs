using System;
using System.Collections.Generic;

namespace GameServer.Utils
{
    public static class ThreadManager
    {
        private static readonly List<Action> toExecuteOnMainThread = new List<Action>();
        private static readonly List<Action> executeCopiedOnMainThread = new List<Action>();
        private static bool actionToExecuteOnMainThread;

        public static void ExecuteOnMainThread(Action action)
        {
            if (action == null) return;
            
            lock (toExecuteOnMainThread)
            {
                toExecuteOnMainThread.Add(action);
                actionToExecuteOnMainThread = true;
            }
        }

        public static void UpdateMain()
        {
            if (!actionToExecuteOnMainThread) return;
            lock (toExecuteOnMainThread)
            {
                executeCopiedOnMainThread.Clear();
                executeCopiedOnMainThread.AddRange(toExecuteOnMainThread);
                toExecuteOnMainThread.Clear();
                actionToExecuteOnMainThread = false;
            }

            foreach (Action t in executeCopiedOnMainThread) t();
        }
    }
}
