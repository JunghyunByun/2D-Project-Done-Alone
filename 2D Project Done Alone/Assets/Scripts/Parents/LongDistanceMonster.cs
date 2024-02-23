using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongDistanceMonster : Monster
{
    [SerializeField] protected float awayRange;
    protected override void IDLE()
    {
        
    }
    protected override void CHASE()
    {
        
    }
    protected override IEnumerator ATTACK()
    {
        yield return null;
    }

    protected override void GETAWAY()
    {
        
    }

    protected override void Detection()
    {
        
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, awayRange);
    }
}
