using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // animator
    private Animator animator = null;


    // attack
    [SerializeField] private bool isAttackKeyFrame = false;
    public bool IsAttackKeyFrame
    {
        get { return isAttackKeyFrame; }
        set { isAttackKeyFrame = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimationChange();
    }


    public void IsDamaged()
    {
        animator.SetBool("isDamaged", true);
    }

    private void AnimationChange()
    {
        if (CheckAnimatorState("takeHit"))
        {
            animator.SetBool("isDamaged", false);
        }
        else if (CheckAnimatorState("attack"))
        {
            animator.SetBool("isAttacking", false);
        }
    }

    private bool CheckAnimatorState(string str)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer." + str))
        {
            return true;
        }
        return false;
    }

    public void Attack()
    {
        animator.SetBool("isAttacking", true);
    }
}
