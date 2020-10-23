using System;
using System.Diagnostics;
using System.Threading.Tasks;
using GameDataStructures;

namespace GameJudge
{
    public class Clock
    {
        private readonly int incrementMs;
        private int redTimeMs;
        private int blueTimeMs;
        private PlayerSide activePlayer = PlayerSide.Red;
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
            redTimeMs = (initialTimeS - incrementS) * 1000;
            blueTimeMs = initialTimeS * 1000;
        }

        public ClockInfo Initialize()
        {
            lastChangeTime = timeProvider.CurrentTime;
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
            Trace.WriteLine($"Checking for loss. {redTimeMs} : {blueTimeMs}");
            if (ActiveTime + lastChangeTime - timeProvider.CurrentTime < 5) Lose(activePlayer);
        }

        private void Lose(PlayerSide loser)
        {
            if (lost) return;
            lost = true;
            lostOnTime(loser);
        }

        private class DefaultTimeProvider : ITimeProvider
        {
            public long CurrentTime => DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            
            public async void SetTimeout(int timeMs, Action callback)
            {
                await Task.Delay(timeMs);
                callback();
            }
        }
    }

    public interface ITimeProvider
    {
        long CurrentTime { get; }
        void SetTimeout(int timeMs, Action callback);
    }
}