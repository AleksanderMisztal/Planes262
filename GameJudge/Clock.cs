using System;
using System.Threading.Tasks;
using GameDataStructures;

namespace GameJudge
{
    public class Clock
    {
        private readonly int incrementMs;
        private int redTimeMs;
        private int blueTimeMs;
        private PlayerSide activePlayer = PlayerSide.Blue;
        private long lastChangeTime = -1;
        private readonly int initialTimeS;
        private readonly int incrementS;
        private readonly ITimeProvider timeProvider;
        private readonly Action<PlayerSide> lostOnTime;
        private bool lost;
    
        private int ActiveTime {
            get => activePlayer == PlayerSide.Blue ? blueTimeMs : redTimeMs; 
            set {
                if (activePlayer == PlayerSide.Blue) blueTimeMs = value;
                else redTimeMs = value;
            }
        }

        public Clock(int initialTimeS, int incrementS, Action<PlayerSide> lostOnTime) 
            : this(initialTimeS, incrementS, new DefaultTimeProvider(), lostOnTime) { }

        public Clock(int initialTimeS, int incrementS, ITimeProvider timeProvider, Action<PlayerSide> lostOnTime)
        {
            this.initialTimeS = initialTimeS;
            this.incrementS = incrementS;
            this.timeProvider = timeProvider;
            this.lostOnTime = lostOnTime;
            incrementMs = incrementS * 1000;
            redTimeMs = initialTimeS * 1000;
            blueTimeMs = initialTimeS * 1000;
        }

        public ClockInfo Initialize()
        {
            lastChangeTime = timeProvider.CurrentTime;
            timeProvider.SetTimeout(ActiveTime, CheckForLoss);
            return new ClockInfo(initialTimeS, incrementS, lastChangeTime);
        }
        
        public TimeInfo ToggleActivePlayer()
        {
            long currentTime = timeProvider.CurrentTime;
            long dt = currentTime - lastChangeTime;
            lastChangeTime = currentTime;
            
            ActiveTime -= (int)dt;
            ActiveTime += incrementMs;
            activePlayer = activePlayer.Opponent();
            timeProvider.SetTimeout(ActiveTime, CheckForLoss);
            
            return new TimeInfo(redTimeMs, blueTimeMs, currentTime);
        }

        private void CheckForLoss()
        {
            Console.WriteLine($"Checking for loss. {redTimeMs} : {blueTimeMs}");
            if (lost || ActiveTime + lastChangeTime - timeProvider.CurrentTime >= 5) return;
            Console.WriteLine("Losing");
            lost = true;            
            lostOnTime(activePlayer);
        }

        private class DefaultTimeProvider : ITimeProvider
        {
            public long CurrentTime => DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            
            public async void SetTimeout(int timeMs, Action callback)
            {
                try
                {
                    await Task.Delay(timeMs);
                    callback();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }

    public interface ITimeProvider
    {
        long CurrentTime { get; }
        void SetTimeout(int timeMs, Action callback);
    }
}