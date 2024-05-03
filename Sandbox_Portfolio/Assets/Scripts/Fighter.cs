using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fighter : MonoBehaviour
{
    private Animator animator;
    public float cooldownTime = 2.0f;
    private float nextFireTime = 0.0f;
    public static int noOfClicks = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 0.9f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 1"))
        {
            animator.SetBool("Attack 1", false);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 2"))
        {
            animator.SetBool("Attack 2", false);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 3"))
        {
            animator.SetBool("Attack 3", false);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 4"))
        {
            animator.SetBool("Attack 4", false);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 5"))
        {
            animator.SetBool("Attack 5", false);
            noOfClicks = 0;
        }

        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }
        if (Time.time > nextFireTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnClick();
            }
        }
    }

    void OnClick()
    {
        lastClickedTime = Time.time;
        noOfClicks++;
        // * Attack 1
        if (noOfClicks == 1)
        {
            animator.SetBool("Attack 1", true);
        }
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 5);
        // * Attack 2
        if(noOfClicks >= 2 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 1"))
        {
            animator.SetBool("Attack 1", false);
            animator.SetBool("Attack 2", true);
        }
        // * Attack 3
        if(noOfClicks >= 3 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 2"))
        {
            animator.SetBool("Attack 2", false);
            animator.SetBool("Attack 3", true);
        }
        // * Attack 4
        if(noOfClicks >= 4 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 3"))
        {
            animator.SetBool("Attack 3", false);
            animator.SetBool("Attack 4", true);
        }
        // * Attack 5
        if(noOfClicks >= 5 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 4"))
        {
            animator.SetBool("Attack 4", false);
            animator.SetBool("Attack 5", true);
        }
    }
}
