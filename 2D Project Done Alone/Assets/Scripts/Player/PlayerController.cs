using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
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

        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;
    }

    protected float GetHorizontal() { return Input.GetAxisRaw("Horizontal"); }

    private void Update()
    {
        transform.Translate(GetHorizontal() * speed * Time.deltaTime, 0f, 0f);

        Animation(GetHorizontal());
    }

    private void OnKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Jump();
        if (Input.GetKeyDown(KeyCode.LeftShift)) Dash();
    }

    private void Jump()
    {
        RaycastHit2D groundHit = Physics2D.Raycast(transform.Find("Ground").position, Vector3.down, 1, LayerMask.GetMask("Ground"));

        if (isJump != true && groundHit.collider != null)
        {
            animator.Play("Jump");

            playerRigid.velocity = new Vector2(0f, Mathf.Pow(speed, 2f));
        }
        Debug.DrawRay(transform.Find("Ground").position, Vector3.down, new Color(0, 1, 0)); 
    }

    private void Dash()
    {
        Vector2 direction = playerSpriteRenderer.flipX ? Vector2.left : Vector2.right;

        playerRigid.AddForce(direction * Mathf.Pow(speed, 2f), ForceMode2D.Force);
    }

    private void Animation(float h)
    {
        if (h != 0)
        {
            animator.Play("Run");

            playerSpriteRenderer.flipX = h < 0;
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D other) { }
}
