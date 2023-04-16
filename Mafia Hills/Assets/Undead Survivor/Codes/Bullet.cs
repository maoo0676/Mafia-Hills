using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static Bullet instance;
    public float damage;
    public int per;
    public int id;

    float timer;
    int waitingTime;

    Rigidbody2D rigid;
    public SpriteRenderer spriter;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        instance = this;
    }

    void Start()
    {
        timer = 0.0f;
        waitingTime = 2;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > waitingTime) {
            if (id != 0 && id != 5 && id != 6 && id != 7) {
                rigid.velocity = Vector2.zero;
                gameObject.SetActive(false);
                timer = 0;
            }
        }
        
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

        if (per > -1)
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
                if (!collision.CompareTag("Enemy") || per == -1 || id == 3 || id == 2)
                    return;

                per--;

                if (per == -1) {
                    rigid.velocity = Vector2.zero;
                    Delete();
                }
                break;
        }

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
