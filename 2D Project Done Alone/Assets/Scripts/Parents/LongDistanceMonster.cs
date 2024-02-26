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
        Collider2D hitRange = Physics2D.OverlapCircle(focus.transform.position, attackRange, layerMask);

        if (hitRange == null) Detection();
        
        state = hitRange != null ? State.ATTACK : State.IDLE;
    }

    protected override void ATTACK()
    {
        if (!isAttack)
        {
            isAttack = true;

            StartCoroutine(AttackCoolDown());
        }
        state = State.GETAWAY;
    }

    protected IEnumerator AttackCoolDown()
    {
        animator.Play("Attack");
        
        yield return new WaitForSeconds(attackTime);

        isAttack = false;

        state = State.IDLE;
    }

    protected override void GETAWAY()
    {
        Collider2D hitRange = Physics2D.OverlapCircle(focus.transform.position, awayRange, layerMask);

        if (hitRange != null)
        {
            isGetAway = false;

            sprite.flipX = playerPos.x < transform.position.x ? false : true;

            Detection();
        }
        else
        {
            isGetAway = true;

            state = State.IDLE;
        }
    }

    protected override void Detection()
    {
        Vector2 raycastOrigin = focus.transform.position;
        Vector2 diagonalRight = new Vector2(Mathf.Cos(310 * Mathf.Deg2Rad), Mathf.Sin(310 * Mathf.Deg2Rad));
        Vector2 diagonalLeft = new Vector2(Mathf.Cos(230 * Mathf.Deg2Rad), Mathf.Sin(230 * Mathf.Deg2Rad));

        RaycastHit2D rightHit = default, leftHit = default;

        if (sprite.flipX)
        {
            leftHit = Physics2D.Raycast(raycastOrigin, Vector2.left, wallRange, LayerMask.GetMask("Ground"));
            leftGroundHit = Physics2D.Raycast(raycastOrigin, diagonalLeft, groundRange, LayerMask.GetMask("Ground"));

            Debug.DrawRay(raycastOrigin, diagonalLeft * groundRange, Color.blue);
            Debug.DrawRay(raycastOrigin, Vector2.left * wallRange, Color.blue);
        }
        else
        {
            rightHit = Physics2D.Raycast(raycastOrigin, Vector2.right, wallRange, LayerMask.GetMask("Ground"));
            rightGroundHit = Physics2D.Raycast(raycastOrigin, diagonalRight, groundRange, LayerMask.GetMask("Ground"));

            Debug.DrawRay(raycastOrigin, diagonalRight * groundRange, Color.blue);
            Debug.DrawRay(raycastOrigin, Vector2.right * wallRange, Color.blue);
        } 

        if (sprite.flipX)
        {
            if (leftGroundHit.collider != null)
            {
                animator.Play("Run");

                transform.position = Vector3.MoveTowards(transform.position, playerPos, isGetAway == true ? speed * Time.deltaTime : -speed * Time.deltaTime);
            } 
            else animator.Play("Idle");
        }
        else
        {
            if (rightGroundHit.collider != null)
            {
                animator.Play("Run");

                transform.position = Vector3.MoveTowards(transform.position, playerPos, isGetAway == true ? speed * Time.deltaTime : -speed * Time.deltaTime);
            } 
            else animator.Play("Idle");
        }

        if (rightHit.collider != null || leftHit.collider != null) rigid.velocity = new Vector2(0f, 7f);
    }

    protected override void DIE()
    {
        
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(focus.transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(focus.transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(focus.transform.position, awayRange);
    }
}
