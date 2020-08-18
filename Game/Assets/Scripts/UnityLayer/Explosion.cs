using UnityEngine;

namespace Planes262.UnityLayer
{
    public class Explosion : MonoBehaviour
    {
        private void Start()
        {
            // TODO: wait for animation end

            Destroy(gameObject);
        }
    }
}