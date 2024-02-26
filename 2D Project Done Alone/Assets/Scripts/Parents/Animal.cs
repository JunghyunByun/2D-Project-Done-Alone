using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animal : MonoBehaviour
{
    // 필요한 상태
    // 1.IDLE : 생성이 후, 가만히 주변을 살핀다. 
    // 2.GETAWAY : 주변에 플레이어가 접근하면 플레이어의 반대로 도망간다.

    protected float speed;
    protected enum State { IDLE, GETAWAY }
    protected State state;
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

    protected virtual void Start() => Init();

    protected virtual void FixedUpdate()
    {
        switch (state)
        {
            case State.IDLE : break;
            case State.GETAWAY : break;
            default : break;
        }
    }

    protected abstract void IDLE();
    protected abstract void GETAWAY();
}
