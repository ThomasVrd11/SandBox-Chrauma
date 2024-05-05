using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo : MonoBehaviour
{
    private Animator animator;
    private int comboIndex = 0;
    //private float lastClickedTime = 0f;
    [SerializeField] private float maxComboDelay = 0.7f;
    private int maxCombo = 5;
    private bool hasAttacked = false;
    private bool continueCombo = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           if (comboIndex > 0)
            {
                continueCombo = true;
            }
            AdvanceCombo();
        }
    }

    private void AdvanceCombo()
    {
        // 0 < combo index +1 < maxCombo normalement
        comboIndex = Mathf.Clamp(comboIndex + 1, 0, maxCombo);
        TriggerAttack(comboIndex);
    }

    void TriggerAttack(int index)
    {
        if (index > 1)
            animator.SetBool($"Attack {index - 1}", false);
        animator.SetBool($"Attack {index}", true);
        Debug.Log("Set " + index);

        if (index >= maxCombo)
            comboIndex = 0;
    }
    // void ResetAttack()
    // {
    //     for (int i = 1; i <= maxCombo; i++)
    //     {
    //         animator.SetBool($"Attack {i}", false);
    //         Debug.Log("Reset " + i);
    //     }
    //     comboIndex = 0;
    //     hasAttacked = false;
    // }
    public void ResetAttackAnimationEvent()
    {
        Debug.Log(continueCombo);
        if (!continueCombo && comboIndex == maxCombo)
        {
            ResetCombo();
        }
        else if (!continueCombo)
        {
            ResetCombo();
        }
        continueCombo = false;
    }

    private void ResetCombo()
    {
        for (int i = 1; i <= maxCombo; i++)
        {
            animator.SetBool($"Attack {i}", false);
        }
        comboIndex = 0;
        Debug.Log("Combo reset by animation event");
    }
}
