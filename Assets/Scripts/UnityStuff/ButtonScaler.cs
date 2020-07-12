using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private readonly static Vector3 highlighted = new Vector3(1.05f, 1.05f, 1.05f);
    private readonly static Vector3 normal = new Vector3(1f, 1f, 1f);

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.localScale = highlighted;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.transform.localScale = normal;
    }
}
