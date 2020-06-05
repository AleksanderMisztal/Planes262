using Scripts.GameLogic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.GameLogic
{
    public class ScoreController : MonoBehaviour
    {
        Text redScoreText;
        Text blueScoreText;

        private int redScore = 0;
        private int blueScore = 0;

        private void Awake()
        {
            redScoreText = transform.Find("Red Score").GetComponent<Text>();
            blueScoreText = transform.Find("Blue Score").GetComponent<Text>();
        }

        void Start()
        {
            redScoreText.text = redScore.ToString();
            blueScoreText.text = blueScore.ToString();
        }


        public void IncrementScore(PlayerId player)
        {
            if (player == PlayerId.Blue)
            {
                blueScoreText.text = (++blueScore).ToString();
            }
            else
            {
                redScoreText.text = (++redScore).ToString();
            }
        }

        public int GetBlueAdvantage()
        {
            return blueScore - redScore;
        }
    }
}
