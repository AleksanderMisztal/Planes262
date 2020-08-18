using UnityEngine;

namespace Planes262.UnityLayer
{
    public class Push3DToFront : MonoBehaviour
    {
        [SerializeField] string layerToPushTo;
        void Start()
        {
            GetComponent<Renderer>().sortingLayerName = layerToPushTo;
        }
    }
}
