using System;
using System.Globalization;
using GameDataStructures;
using UnityEngine;
using UnityEngine.UI;

namespace Planes262.UnityLayer
{
    public class Clock : MonoBehaviour
    {
        [SerializeField] private Text redTimeText;
        [SerializeField] private Text blueTimeText;

        [SerializeField] private float initialTime;
        [SerializeField] private float increment;

        public Action<PlayerSide> LostOnTime;

        private float redTime;
        private float blueTime;

        private PlayerSide activePlayer = PlayerSide.Red;
        public bool IsPlaying { get; set; }

        private void Start()
        {
            redTime = initialTime - increment;
            blueTime = initialTime;
            
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            redTimeText.text = ((int)redTime).ToString(CultureInfo.CurrentCulture);
            blueTimeText.text = ((int)blueTime).ToString(CultureInfo.CurrentCulture);
        }

        private void Update()
        {
            if (!IsPlaying) return;
            if (activePlayer == PlayerSide.Red)
            {
                redTime -= Time.deltaTime;
                if (redTime <= 0)
                {
                    redTime = 0;
                    LostOnTime(PlayerSide.Red);
                }
            }
            else
            {
                blueTime -= Time.deltaTime;
                if (blueTime <= 0)
                {
                    blueTime = 0;
                    LostOnTime(PlayerSide.Blue);
                }
            }
            UpdateDisplay();
        }

        public void ToggleActivePlayer()
        {
            if (activePlayer == PlayerSide.Red) redTime += increment;
            if (activePlayer == PlayerSide.Blue) blueTime += increment;

            activePlayer = activePlayer.Opponent();
            UpdateDisplay();
        }
    }
}