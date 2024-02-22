using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongDistanceMonster : MonoBehaviour
{
    [SerializeField] protected float size, attackSize, speed, HP, attackDelay, raySize, groundSize, getAwaySize;
    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected Transform myfocus;
    protected enum State { IDLE, CHASE, ATTACK, GETAWAY, DIE }
    protected RaycastHit2D rightGroundHit, leftGroundHit;
    protected SpriteRenderer sprite;
    protected State state;
    protected Transform target;
    protected Animator animator;
    protected Rigidbody2D rb;
    protected bool isAttack, isGetAway, isArrow, isGetAwayGroundStop;
    protected string prefabName;

    protected virtual void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        state = State.IDLE;

        StartCoroutine(StateUpdate());
    }

    protected IEnumerator StateUpdate()
    {
        while (true)
        {
            if (HP <= 0) state = State.DIE;

            if (!isGetAway)
            {
                if (GameObject.Find("Player").transform.position.x < transform.position.x) sprite.flipX = true;
                else sprite.flipX = false;
            }
            
            switch (state)
            {
                case State.IDLE : IDLE(); break;
                case State.CHASE : CHASE(); break;
                case State.ATTACK : if (isAttack) StartCoroutine(ATTACK()); break;
                case State.GETAWAY : GETAWAY(); break;
                case State.DIE : StartCoroutine(DIE()); break;
                default : break;
            }
            yield return null;
        }
    }

    protected void IDLE()
    {
        isAttack = false; 

        Collider2D hit = Physics2D.OverlapCircle(myfocus.position, size, layerMask);

        if (hit == null) animator.SetBool("isRun", false);
        else 
        {
            target = hit.gameObject.transform;
            isGetAwayGroundStop = false;
            state = State.CHASE;
        }
    }

    protected void CHASE()
    {
        Collider2D hit = Physics2D.OverlapCircle(myfocus.position, attackSize, layerMask);

        if (hit == null) AmbientDetection();
        else
        {
            animator.SetBool("isRun", false);
            isAttack = true;
            isArrow = false;
        }
        state = isAttack ? State.ATTACK : State.IDLE;
    }

    private void AmbientDetection()
    {
        Vector2 raycastOrigin = myfocus.position;
        Vector2 diagonalRight = new Vector2(Mathf.Cos(310 * Mathf.Deg2Rad), Mathf.Sin(310 * Mathf.Deg2Rad));
        Vector2 diagonalLeft = new Vector2(Mathf.Cos(230 * Mathf.Deg2Rad), Mathf.Sin(230 * Mathf.Deg2Rad));

        RaycastHit2D rightHit = default;
        RaycastHit2D leftHit = default;

        if (sprite.flipX)
        {
            leftHit = Physics2D.Raycast(raycastOrigin, Vector2.left, raySize, LayerMask.GetMask("Ground"));
            leftGroundHit = Physics2D.Raycast(raycastOrigin, diagonalLeft, groundSize, LayerMask.GetMask("Ground"));

            Debug.DrawRay(raycastOrigin, diagonalLeft * groundSize, Color.blue);
            Debug.DrawRay(raycastOrigin, Vector2.left * raySize, Color.blue);
        }
        else
        {
            rightHit = Physics2D.Raycast(raycastOrigin, Vector2.right, raySize, LayerMask.GetMask("Ground"));
            rightGroundHit = Physics2D.Raycast(raycastOrigin, diagonalRight, groundSize, LayerMask.GetMask("Ground"));

            Debug.DrawRay(raycastOrigin, diagonalRight * groundSize, Color.blue);
            Debug.DrawRay(raycastOrigin, Vector2.right * raySize, Color.blue);
        } 
        if (!isGetAwayGroundStop) animator.SetBool("isRun", true);

        if (!isGetAway)
        {
            if (sprite.flipX)
            {
                if (leftGroundHit.collider != null) transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                else animator.SetBool("isRun", false);
            }
            else
            {
                if (rightGroundHit.collider != null) transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                else animator.SetBool("isRun", false);
            }
        }
        else
        {
            if (rightGroundHit.collider == null) isGetAwayGroundStop = true;
            else if (leftGroundHit.collider == null) isGetAwayGroundStop = true;
            else isGetAwayGroundStop = false;
        }
        if (rightHit.collider != null || leftHit.collider != null) rb.velocity = new Vector2(0f, 7f);
    }

    protected IEnumerator ATTACK()
    {
        if (isAttack)
        {
            isAttack = false;
            animator.SetBool("isAttack", true);

            yield return new WaitForSeconds(1.8f);

            if (!isArrow)
            {
                isArrow = true;
                Instantiate(Resources.Load($"Prefabs/{prefabName}"), myfocus.position, Quaternion.identity);
                animator.SetBool("isAttack", false);
            }
            yield return new WaitForSeconds(attackDelay);

            state = State.GETAWAY;
        }
    }

    protected void GETAWAY()
    {
        Collider2D hit = Physics2D.OverlapCircle(myfocus.position, getAwaySize, layerMask);

        if (hit != null)
        {
            isGetAway = true;
            bool playerPos = transform.position.x < GameObject.Find("Player").transform.position.x ? true : false;
            sprite.flipX = playerPos ? true : false;

            if (!isGetAwayGroundStop)
            {
                animator.SetBool("isRun", true);
                transform.Translate((playerPos ? -speed : speed) * Time.deltaTime, 0f, 0f);
            } 
            else animator.SetBool("isRun", false);

            AmbientDetection();
        }
        else
        {
            animator.SetBool("isRun", false);
            isGetAway = false;

            state = State.IDLE;
        }
    }

    protected IEnumerator DIE()
    {
        animator.SetTrigger("isDie");

        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(myfocus.position, size);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(myfocus.position, attackSize);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(myfocus.position, getAwaySize);
    }

    protected void OnTriggerEnter2D(Collider2D other) { if (other.gameObject.CompareTag("Sword1")) StartCoroutine(TakeDamage(GameManager.Instance.playerDamage)); }

    private IEnumerator TakeDamage(float Damage)
    {
        sprite.color = Color.red;
        HP -= Damage;

        yield return new WaitForSeconds(0.25f);
        
        sprite.color = Color.white;
    }
}
