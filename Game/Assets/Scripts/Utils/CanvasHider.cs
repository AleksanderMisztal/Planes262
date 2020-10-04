using UnityEngine;

namespace Planes262.Utils
{
    public class CanvasHider : MonoBehaviour
    {
        [SerializeField]
        private string sortingLayer;
        [SerializeField]
        private int orderInLayer;


        private void Start()
        {
            Canvas myCanvas = GetComponent<Canvas>();
            myCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            myCanvas.worldCamera = Camera.main;

            myCanvas.sortingLayerName = sortingLayer;
            myCanvas.sortingOrder = orderInLayer;
        }
    }
}