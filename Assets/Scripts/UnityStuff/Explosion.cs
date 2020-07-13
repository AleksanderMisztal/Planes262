using Cysharp.Threading.Tasks;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion;
    private Animator animator;


    private async void Start()
    {
        animator = GetComponent<Animator>();
        await UniTask.WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("explosion"));
        await UniTask.WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1
            && !animator.IsInTransition(0));

        Destroy(gameObject);
    }
}
