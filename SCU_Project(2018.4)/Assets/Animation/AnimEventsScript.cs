using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventsScript : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void AttackReset()
    {
        //Debug.Log("Attack Reset");
        animator.SetBool("Attack", false);

    }

    void GotHitReset()
    {
        //Debug.Log("GotHit Reset");
        animator.SetBool("GotHit", false);
    }
}
