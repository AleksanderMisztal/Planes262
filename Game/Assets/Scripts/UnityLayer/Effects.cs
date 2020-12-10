using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Planes262.UnityLayer
{
    public class Effects : MonoBehaviour
    {
        [SerializeField] private GameObject explosionPrefab;
        [SerializeField] private GameObject machineFirePrefab;

        public void Explode(Vector3 position, int times)
        {
            while (times --> 0)
            {
                Vector3 randomOffset = position + Random.insideUnitSphere;
                GameObject explosion = Instantiate(explosionPrefab, randomOffset, Quaternion.identity);
                StartCoroutine(WaitDestroy(1, explosion));
            }
        }

        public void Fire(Vector3 position)
        {
            GameObject machineFire = Instantiate(machineFirePrefab, position, Quaternion.identity);
            StartCoroutine(WaitDestroy(1, machineFire));
        }
        
        private static IEnumerator WaitDestroy(float seconds, Object gObject)
        {
            yield return new WaitForSeconds(seconds);
            Destroy(gObject);
        }
    }
}
