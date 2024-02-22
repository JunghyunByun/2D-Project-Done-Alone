using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float arrowSpeed = 7.5f;
    private SpriteRenderer arrowSprite;
    private Rigidbody2D arrowRigid;

    void Start()
    {
        arrowSprite = GetComponent<SpriteRenderer>();
        arrowRigid = GetComponent<Rigidbody2D>();

        arrowSprite.flipX = GameObject.Find("Skeleton").GetComponent<SpriteRenderer>().flipX;

        Vector2 direction = (GameObject.Find("Player").transform.position - transform.position).normalized;

        arrowRigid.velocity = direction * arrowSpeed;

        Destroy(gameObject, 5f);
    }
}
