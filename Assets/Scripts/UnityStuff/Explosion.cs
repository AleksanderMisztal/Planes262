﻿using Cysharp.Threading.Tasks;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private async void Start()
    {
        Animator animator = GetComponent<Animator>();
        await UniTask.WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("explosion"));
        await UniTask.WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1
            && !animator.IsInTransition(0));

        Destroy(gameObject);
    }
}
