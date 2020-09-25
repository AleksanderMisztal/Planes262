using System;
using GameDataStructures;

namespace GameServer.Matchmaking
{
    public class Clock
    {
        private int redTimeMs;
        private int blueTimeMs;
        private PlayerSide activePlayer = PlayerSide.Red;
        private long lastChangeTime = -1;
    
        public Clock(int initialTimeS, int incrementS)
        {
            redTimeMs = (initialTimeS - incrementS) * 1000;
            blueTimeMs = initialTimeS * 1000;
        }

        public void OnMoveMade()
        {
            if (lastChangeTime == -1) lastChangeTime = CurrentTime;
        }
        
        public TimeInfo ToggleActivePlayer()
        {
            long currentTime = CurrentTime;
            long dt = currentTime - lastChangeTime;
            lastChangeTime = currentTime;
            
            if (activePlayer == PlayerSide.Red) redTimeMs -= (int)dt;
            if (activePlayer == PlayerSide.Blue) blueTimeMs -= (int)dt;
            activePlayer = activePlayer.Opponent();
            
            return new TimeInfo(redTimeMs, blueTimeMs, currentTime);
        }
        
        private static long CurrentTime => DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    }
}