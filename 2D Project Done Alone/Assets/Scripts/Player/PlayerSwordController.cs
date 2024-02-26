using System.Collections;
using UnityEngine;

public class PlayerSwordController : PlayerController
{
    private Transform swordFocus;
    private GameObject swordEffect;
    private AudioSource swordAudio;
    private bool isAttack;

    protected override void Start()
    {
        base.Start();

        swordFocus = transform.Find("SwordFocus");
        swordEffect = transform.Find("Sword Effect").gameObject;
        swordAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        SwordFlip(GetHorizontal());

        if (Input.GetMouseButtonDown(0) && !isAttack)
        {
            isAttack = true;

            swordAudio.Play();

            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        Quaternion r = Quaternion.Euler(0f, 0f, playerSpriteRenderer.flipX ? 45f : -45f);
        Vector3 pos = playerSpriteRenderer.flipX ? new Vector3(-1.18f, -0.3f, 0f) : new Vector3(1.18f, -0.3f, 0f);

        swordFocus.transform.localRotation = r; 
        swordEffect.transform.localPosition = pos;

        swordEffect.SetActive(true);
        yield return new WaitForSeconds(0.15f);

        swordFocus.rotation = Quaternion.identity;
        swordEffect.SetActive(false);

        isAttack = false;
    }

    private void SwordFlip(float h)
    {
        if (h != 0)
        {
            float sword_x = h > 0 ? 0.589f : -0.589f;

            swordFocus.localScale = new Vector3(h > 0 ? 1f : -1f, 1f, 1f);
            swordFocus.localPosition = new Vector3(sword_x, swordFocus.localPosition.y, swordFocus.localPosition.z);
            swordEffect.transform.localScale = h > 0 ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
        } 
    }

    protected override void OnCollisionEnter2D(Collision2D other) { }
}
