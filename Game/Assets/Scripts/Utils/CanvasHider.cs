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
            myCanvas.renderMode = RenderMode.WorldSpace;
            myCanvas.worldCamera = FindObjectOfType<Camera>();

            myCanvas.sortingLayerName = sortingLayer;
            myCanvas.sortingOrder = orderInLayer;
        }
    }
}