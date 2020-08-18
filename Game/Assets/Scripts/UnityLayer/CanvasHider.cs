using UnityEngine;

namespace Planes262.UnityLayer
{
    public class CanvasHider : MonoBehaviour
    {
        [SerializeField]
        private string sortingLayer;
        [SerializeField]
        private int orderInLayer;


        private void Start()
        {
            var myCanvas = GetComponent<Canvas>();
            myCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            myCanvas.worldCamera = Camera.main;

            myCanvas.sortingLayerName = sortingLayer;
            myCanvas.sortingOrder = orderInLayer;
        }
    }
}