using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static Bullet instance;
    public float damage;
    public int per;
    public int id;

    Rigidbody2D rigid;
    public SpriteRenderer spriter;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        instance = this;
    }

    void Update()
    {   
        if (id == 5) {
            transform.Translate(Vector3.down * 3 * Time.deltaTime, Space.World);
        }
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        if (id == 5)
            gameObject.SetActive(true);

        this.damage = damage;
        this.per = per;

        if (per >= 0)
            rigid.velocity = dir * 15f;

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (id)
        {
            case 5:
                if (!collision.CompareTag("Player"))
                    return;
                rigid.velocity = Vector2.zero;
                Delete();

                GameManager.instance.health += GameManager.instance.maxHealth / 100 * damage;
                break;
            case 10 :
                if (!collision.CompareTag("Enemy"))
                    return;
                
                rigid.velocity = Vector2.zero;
                Delete();

                GameManager.instance.health += GameManager.instance.maxHealth / 100 * damage;
                break;
            default:
                if (!collision.CompareTag("Enemy") || per == -100 || id == 3 || id == 2)
                    return;

                per--;

                if (per < 0) {
                    rigid.velocity = Vector2.zero;
                    Delete();
                }
                break;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area") || per == -100)
                    return;

        rigid.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    void Delete()
    {
        gameObject.SetActive(false);
    }

    public void Batch()
    {
        StartCoroutine(Resurrection());
    }

    IEnumerator Resurrection()
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
