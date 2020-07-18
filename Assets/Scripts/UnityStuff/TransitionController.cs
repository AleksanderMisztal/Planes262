﻿using Cysharp.Threading.Tasks;
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

    public static async UniTask StartTransition()
    {
        instance.animator.SetBool("shouldContinue", false);
        instance.animator.SetBool("shouldStart", true);

        await UniTask.WaitUntil(() => instance.animator.GetCurrentAnimatorStateInfo(0).IsName("transition start"));
        await UniTask.WaitUntil(() =>
            instance.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1
            && !instance.animator.IsInTransition(0));
    }

    public static async UniTask EndTransition()
    {
        instance.animator.SetBool("shouldStart", false);
        instance.animator.SetBool("shouldContinue", true);

        await UniTask.WaitUntil(() => instance.animator.GetCurrentAnimatorStateInfo(0).IsName("transition end"));
        await UniTask.WaitUntil(() => 
            instance.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 
            && !instance.animator.IsInTransition(0));
    }
}