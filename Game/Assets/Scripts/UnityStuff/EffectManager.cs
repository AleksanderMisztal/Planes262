using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private static EffectManager instance;

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

    public static void InstantiateExplosion(Vector3 position)
    {
        Instantiate(instance.explosion, position, Quaternion.identity);
    }
}
