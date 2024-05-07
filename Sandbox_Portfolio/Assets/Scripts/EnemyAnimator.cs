using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
private Animator animator;
private Rigidbody rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        bool isWalking = Mathf.Abs(rb.velocity.magnitude) > 0.1f;

        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isIdle", !isWalking);
    }
}
