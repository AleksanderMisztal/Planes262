using UnityEngine;

namespace Planes262.UnityLayer
{
    public class Effects : MonoBehaviour
    {
        private static Effects instance;

        [SerializeField]
        private GameObject explosion;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Debug.Log("Instance already exists, destroying this...");
                Destroy(this);
            }
        }

        public static void Explode(Vector3 position, int times)
        {
            while (times-- > 0)
            {
                var randomlyOffset = position + Random.insideUnitSphere;
                Instantiate(instance.explosion, randomlyOffset, Quaternion.identity);
            }
        }
    }
}