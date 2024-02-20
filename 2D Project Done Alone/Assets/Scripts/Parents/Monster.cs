using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] protected float size, attackSize, speed, HP, attackDelay;
    [SerializeField] protected LayerMask layerMask;
    protected enum State { IDLE, CHASE, ATTACK, DIE }
    protected SpriteRenderer sprite;
    protected State state;
    protected Transform target;
    protected Animator animator;
    protected bool isAttack;

    protected virtual void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        state = State.IDLE;

        StartCoroutine(StateUpdate());
    }

    protected IEnumerator StateUpdate()
    {
        while (true)
        {
            if (HP <= 0) state = State.DIE;

            if (GameObject.Find("Player").GetComponent<Transform>().transform.position.x < transform.position.x) sprite.flipX = true;
            else sprite.flipX = false;
            
            switch (state)
            {
                case State.IDLE : IDLE(); break;
                case State.CHASE : CHASE(); break;
                case State.ATTACK : StartCoroutine(ATTACK()); break;
                case State.DIE : DIE(); break;
                default : break;
            }
            yield return null;
        }
    }

    protected void IDLE()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, size, layerMask);

        if (hit == null) animator.SetBool("isRun", false);
        else 
        {
            state = State.IDLE;

            target = hit.gameObject.transform;

            state = State.CHASE;
        }
    }

    protected void CHASE()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, attackSize, layerMask);

        if (hit == null)
        {
            animator.SetBool("isRun", true);
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime); 
        } 
        else
        {
            animator.SetBool("isRun", false);
            isAttack = true;
        }

        state = isAttack ? State.ATTACK : State.IDLE;
    } 

    protected IEnumerator ATTACK()
    {
        if (isAttack)
        {
            animator.SetBool("isAttack", true);

            yield return new WaitForSeconds(attackDelay);

            animator.SetBool("isAttack", false);

            state = State.IDLE;
            isAttack = false; 
        }
    }

    protected void DIE()
    {
        
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, size);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackSize);
    }

    protected void OnTriggerEnter2D(Collider2D other) { if (other.gameObject.CompareTag("Sword1")) HP -= GameManager.Instance.playerDamage; }
}
