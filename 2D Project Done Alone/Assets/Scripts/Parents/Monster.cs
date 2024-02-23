using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    [SerializeField] protected float speed, chaseRange, attackRange, wallRange, groundRange;
    [SerializeField] protected LayerMask layerMask;
    protected enum State { IDLE, CHASE, ATTACK, GETAWAY }
    protected State state;
    protected RaycastHit2D rightGroundHit, leftGroundHit;
    protected Vector3 playerPos;
    protected SpriteRenderer sprite;
    protected Rigidbody2D rigid;
    protected Animator animator;

    protected void Init()
    {
        sprite = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        state = State.IDLE;
    }

    protected virtual void Start()
    {
        Init();
    }

    protected virtual void FixedUpdate()
    {
        playerPos = GameObject.Find("Player").transform.position;

        sprite.flipX = playerPos.x > transform.position.x ? false : true;

        switch (state)
        {
            case State.IDLE : IDLE(); break;
            case State.CHASE : CHASE(); break;
            case State.ATTACK : StartCoroutine(ATTACK()); break;
            case State.GETAWAY : GETAWAY(); break;
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

    protected abstract void IDLE();
    protected abstract void CHASE();
    protected abstract IEnumerator ATTACK(); 
    protected abstract void Detection();
    protected virtual void GETAWAY() { }
}
