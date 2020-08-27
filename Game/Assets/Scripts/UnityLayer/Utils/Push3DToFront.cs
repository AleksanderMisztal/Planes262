﻿using UnityEngine;

namespace Planes262.UnityLayer.Utils
{
    public class Push3DToFront : MonoBehaviour
    {
        [SerializeField] private string layerToPushTo;

        private void Start()
        {
            GetComponent<Renderer>().sortingLayerName = layerToPushTo;
        }
    }
}