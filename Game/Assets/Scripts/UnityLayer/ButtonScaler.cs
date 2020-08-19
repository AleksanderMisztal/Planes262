using UnityEngine;
using UnityEngine.EventSystems;

namespace Planes262.UnityLayer
{
    public class ButtonScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private static readonly Vector3 highlighted = new Vector3(1.05f, 1.05f, 1.05f);
        private static readonly Vector3 normal = new Vector3(1f, 1f, 1f);

        public void OnPointerEnter(PointerEventData eventData)
        {
            gameObject.transform.localScale = highlighted;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            gameObject.transform.localScale = normal;
        }
    }
}