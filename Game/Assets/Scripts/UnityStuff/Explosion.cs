using UnityEngine;

public class Explosion : MonoBehaviour
{
    private void Start()
    {
        // TODO: wait for animation end

        Destroy(gameObject);
    }
}
