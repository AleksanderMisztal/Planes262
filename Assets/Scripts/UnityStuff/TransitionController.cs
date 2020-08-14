using UnityEngine;

public class TransitionController : MonoBehaviour
{
    private static TransitionController instance;

    private Animator animator;
    private Canvas canvas;

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

    private void Start()
    {
        animator = GetComponent<Animator>();
        canvas = GetComponent<Canvas>();
        canvas.sortingLayerName = "Transition";
        canvas.sortingOrder = 20;

    }

    public static void StartTransition()
    {
        instance.animator.SetBool("shouldContinue", false);
        instance.animator.SetBool("shouldStart", true);

        // May want to be able to await animation end
    }

    public static void EndTransition()
    {
        instance.animator.SetBool("shouldStart", false);
        instance.animator.SetBool("shouldContinue", true);

        // May want to be able to await animation end
    }
}
