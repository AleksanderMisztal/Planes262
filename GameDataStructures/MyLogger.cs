using System.Diagnostics;

namespace GameDataStructures
{
    public static class MyLogger
    {
        public static IMyLogger myLogger { private get; set; } = new DefaultMyLogger();
        
        public static void Log(string message)
        {
            myLogger.Log(message);
        }

        private class DefaultMyLogger : IMyLogger
        {
            public void Log(string message) => Trace.WriteLine(message);
        }
    }

    public interface IMyLogger
    {
        void Log(string message);
    }
    
    
}