using UnityEngine;
using UnityEngine.UI;

namespace Planes262.UnityLayer
{
    public class BackgroundManager : MonoBehaviour
    {
        private const string path = "Backgrounds/";

        public void SetBackground(string levelName)
        {
            Sprite background = Resources.Load<Sprite>(path + levelName);
            Debug.Log(background);
            GetComponent<Image>().sprite = background;
        }
    }
}