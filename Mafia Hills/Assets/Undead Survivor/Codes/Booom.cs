using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booom : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rigid;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        anim.SetBool("Boom", false);
        transform.localScale = new Vector2(1, 1);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;

        anim.SetBool("Boom", true);
        rigid.velocity = Vector2.zero;
        transform.localScale = new Vector2(GameManager.instance.boomScale, GameManager.instance.boomScale);
    }
}
