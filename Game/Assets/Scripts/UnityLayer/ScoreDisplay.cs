using UnityEngine;
using UnityEngine.UI;

namespace Planes262.UnityLayer
{
    public class ScoreDisplay : MonoBehaviour
    {
        [SerializeField] private Text scoreText;
        private string redName = "";
        private string blueName = "";

        public ScoreDisplay(Text scoreText)
        {
            this.scoreText = scoreText;
        }

        public void SetNames(string red, string blue)
        {
            redName = red;
            blueName = blue;
        }

        public void Set(int red, int blue)
        {
            scoreText.text = blueName + " " + blue + " : " + red + " " + redName;
        }
    }
}
