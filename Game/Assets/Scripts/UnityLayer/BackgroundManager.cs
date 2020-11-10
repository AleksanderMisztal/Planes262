using System.Collections;
using Planes262.LevelEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Planes262.UnityLayer
{
    public class BackgroundManager : MonoBehaviour
    {
        private const string path = "Backgrounds/";

        public void SetBackground(string backgroundName)
        {
            LevelConfig.background = backgroundName;
            Sprite background = Resources.Load<Sprite>(path + backgroundName);
            transform.GetChild(0).GetComponent<Image>().sprite = background;
        }

        private IEnumerator Co_DetachBackground()
        {
            yield return null;
            Canvas canvas = GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
        }

        public void DetachBackground()
        {
            StartCoroutine(Co_DetachBackground());
        }
    }
}