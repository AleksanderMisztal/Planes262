using UnityEngine;

namespace Scripts.UnityStuff
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
