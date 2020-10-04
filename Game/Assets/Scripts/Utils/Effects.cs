using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Planes262.Utils
{
    public class Effects : MonoBehaviour
    {
        [SerializeField] private GameObject explosionPrefab;

        public void Explode(Vector3 position, int times)
        {
            while (times --> 0)
            {
                Vector3 randomOffset = position + Random.insideUnitSphere;
                GameObject explosion = Instantiate(explosionPrefab, randomOffset, Quaternion.identity);
                StartCoroutine(WaitDestroy(1, explosion));
            }
        }
        
        private static IEnumerator WaitDestroy(float seconds, Object gObject)
        {
            yield return new WaitForSeconds(seconds);
            Destroy(gObject);
        }
    }
}
