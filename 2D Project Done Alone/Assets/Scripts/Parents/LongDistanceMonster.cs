using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongDistanceMonster : Monster
{
    [SerializeField] protected GameObject focus;
    [SerializeField] protected float awayRange;
    protected string prefabName;

    protected override void IDLE()
    {
        Collider2D hitRange = Physics2D.OverlapCircle(focus.transform.position, chaseRange, layerMask);

        if (hitRange == null) animator.Play("Idle");

        state = hitRange != null ? State.CHASE : State.IDLE;
    }

    protected override void CHASE()
    {
        Collider2D hitRange = Physics2D.OverlapCircle(transform.position, attackRange, layerMask);

        if (hitRange == null)
        {
            animator.Play("Run");

            Detection();
        } 
        state = hitRange != null ? State.ATTACK : State.IDLE;
    }

    protected override IEnumerator ATTACK()
    {
        animator.Play("Attack");

        yield return null;
    }

    protected override void GETAWAY()
    {
        
    }

    protected override void Detection()
    {
        
    }

    protected override void DIE()
    {
        
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, awayRange);
    }
}
