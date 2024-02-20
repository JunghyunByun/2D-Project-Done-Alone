using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 5f;
    private Rigidbody2D playerRigid;
    private Animator animator;
    public SpriteRenderer playerSpriteRenderer;
    protected float playerX;
    private bool isJump;
    
    protected virtual void Start()
    {
        playerRigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected float GetHorizontal() { return Input.GetAxisRaw("Horizontal"); }

    private void Update()
    {
        transform.Translate(GetHorizontal() * speed * Time.deltaTime, 0f, 0f);

        Jump();
        Animation(GetHorizontal());
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isJump != true)
        {
            isJump = true;

            animator.SetBool("isJump", isJump);

            playerRigid.velocity = new Vector2(0f, Mathf.Pow(speed, 2f));
        }
    }

    private void Animation(float h)
    {
        if (h != 0)
        {
            animator.SetBool("isRun", true);

            playerSpriteRenderer.flipX = h < 0;
        }
        else animator.SetBool("isRun", false);
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            isJump = false;
            
            animator.SetBool("isJump", isJump);
        }
    }
}
