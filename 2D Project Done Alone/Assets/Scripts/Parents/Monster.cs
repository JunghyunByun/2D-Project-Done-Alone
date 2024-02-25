using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    [SerializeField] protected float speed, HP, attackTime, chaseRange, attackRange, wallRange, groundRange;
    [SerializeField] protected LayerMask layerMask;
    protected enum State { IDLE, CHASE, ATTACK, GETAWAY, DIE }
    protected State state;
    protected RaycastHit2D rightGroundHit, leftGroundHit;
    protected Vector3 playerPos;
    protected SpriteRenderer sprite;
    protected Rigidbody2D rigid;
    protected Animator animator;
    protected bool isAttack, isGetAway;

    protected void Init()
    {
        sprite = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        state = State.IDLE;
    }

    protected virtual void Start() => Init();
    
    protected virtual void FixedUpdate()
    {
        playerPos = GameObject.Find("Player").transform.position;

        if (isGetAway) sprite.flipX = playerPos.x > transform.position.x ? false : true;

        if (HP <= 0) state = State.DIE;
        
        switch (state)
        {
            case State.IDLE : IDLE(); break;
            case State.CHASE : CHASE(); break;
            case State.ATTACK : if (!isAttack) ATTACK(); break;
            case State.GETAWAY : GETAWAY(); break;
            case State.DIE : DIE(); break;
            default : break;
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Sword1")) 
        {
            sprite.color = Color.red;
            HP -= GameManager.Instance.playerDamage;
            StartCoroutine(ResetColorAfterDelay());
        }
    }

    protected IEnumerator ResetColorAfterDelay()
    {
        yield return new WaitForSeconds(0.25f);
        sprite.color = Color.white;
    }

    protected abstract void IDLE();
    protected abstract void CHASE();
    protected abstract void ATTACK(); 
    protected abstract void Detection();
    protected abstract void DIE();
    protected virtual void GETAWAY() { }
}
