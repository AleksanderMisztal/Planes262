using System;
using System.Collections.Generic;
using System.Diagnostics;
using GameDataStructures;
using GameJudge;
using NUnit.Framework;

namespace JudgeTests
{
    public class ClockTests
    {
        private class MockTimeProvider : ITimeProvider
        {
            private readonly List<Func<bool>> timedActions = new List<Func<bool>>();

            public long CurrentTime { get; private set; } = 0;

            public void Inc(int s)
            {
                if (s <= 0) throw new Exception("Can't go back in time");
                CurrentTime += 1000 * s;
                for (int i = timedActions.Count - 1; i >= 0; i--)
                    if (timedActions[i]())
                        timedActions.RemoveAt(i);
            }

            public void SetTimeout(int timeMs, Action callback)
            {
                long calledAt = CurrentTime;

                bool TimedAction()
                {
                    if (CurrentTime < calledAt + timeMs) return false;
                    Trace.WriteLine($"Time is {CurrentTime}. Calling...");
                    callback();
                    return true;
                }

                timedActions.Add(TimedAction);
            }
        }
        
        bool lost;
        private PlayerSide loser;
        private int timesLost;
        private Clock clock;
        private MockTimeProvider tp;

        private void SetUp(int inc)
        {
            lost = false;
            timesLost = 0;
            tp = new MockTimeProvider();
            clock = new Clock(10, inc, tp, LostOnTime);

            clock.Initialize();
            clock.ToggleActivePlayer();
        }

        private void SetUp() => SetUp(0);

        private TimeInfo Toggle() => clock.ToggleActivePlayer();

        private void LostOnTime(PlayerSide side)
        {
            lost = true;
            loser = side;
            timesLost++;
        }

        [Test]
        public void TestTimeRunning()
        {
            SetUp();
            tp.Inc(5);
            TimeInfo ti = Toggle();
            Assert.AreEqual(5000, ti.BlueTimeMs);
        }

        [Test]
        public void TestLosing()
        {
            SetUp();
            
            tp.Inc(11);
            
            Assert.IsTrue(lost);
        }

        [Test]
        public void TestLosesOnce()
        {
            SetUp();
            
            Toggle();
            Toggle();
            Toggle();
            Toggle();
            tp.Inc(11);
            
            Assert.IsTrue(lost);
            Assert.AreEqual(PlayerSide.Blue, loser);
            Assert.AreEqual(1, timesLost);
        }

        [Test]
        public void TestClockWithIncrement()
        {
            SetUp(2);
            
            tp.Inc(3);
            TimeInfo ti = Toggle();
            Assert.AreEqual(9000, ti.BlueTimeMs);
            tp.Inc(5);
            ti = Toggle();
            Assert.AreEqual(7000, ti.RedTimeMs);
            
            tp.Inc(5);
            Toggle();
            tp.Inc(4);
            Toggle();
            
            Assert.IsFalse(lost);
            
            tp.Inc(6);
            Toggle();
            
            Assert.IsTrue(lost);
            Assert.AreEqual(PlayerSide.Blue, loser);
        }
    }
}
