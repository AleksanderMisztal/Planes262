using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push3DToFront : MonoBehaviour
{
    [SerializeField] string layerToPushTo;
    void Start()
    {
        GetComponent<Renderer>().sortingLayerName = layerToPushTo;
    }
}
