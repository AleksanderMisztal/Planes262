using System;
using System.Globalization;
using GameDataStructures;
using UnityEngine;
using UnityEngine.UI;

namespace Planes262.UnityLayer
{
    public class ClockDisplay : MonoBehaviour
    {
        [SerializeField] private Text redTimeText;
        [SerializeField] private Text blueTimeText;

        private float initialTime;
        private float increment;

        private float redTime;
        private float blueTime;

        private PlayerSide activePlayer = PlayerSide.Red;
        
        private float ActiveTime {
            get => activePlayer == PlayerSide.Blue ? blueTime : redTime; 
            set {
                if (activePlayer == PlayerSide.Blue) blueTime = value;
                else redTime = value;
            }
        }

        public void ResetTime(ClockInfo clockInfo)
        {
            initialTime = clockInfo.InitialTimeS;
            increment = clockInfo.IncrementS;

            int dt = (int)(CurrentTime - clockInfo.StartTimestamp);
            
            redTime = initialTime - increment;
            blueTime = initialTime - dt / 1000f;
            
            UpdateDisplay();
        }

        private void Update()
        {
            ActiveTime -= Time.unscaledDeltaTime;
            if (ActiveTime < 0) ActiveTime = 0;
            UpdateDisplay();
        }

        public void ToggleActivePlayer(TimeInfo timeInfo)
        {
            float totalTime = blueTime + redTime + increment;
            if (activePlayer == PlayerSide.Red)
            {
                redTime = timeInfo.RedTimeMs / 1000f;
                blueTime = totalTime - redTime;
            }
            else
            {
                blueTime = timeInfo.BlueTimeMs / 1000f;
                redTime = totalTime - blueTime;
            }
            activePlayer = activePlayer.Opponent();
            UpdateDisplay();
        }
        
        private void UpdateDisplay()
        {
            redTimeText.text = ((int)redTime).ToString(CultureInfo.CurrentCulture);
            blueTimeText.text = ((int)blueTime).ToString(CultureInfo.CurrentCulture);
        }
        
        private long CurrentTime => DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    }
}