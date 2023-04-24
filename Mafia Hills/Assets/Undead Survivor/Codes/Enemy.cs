using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxhealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;
    int x = 0;
    float timer;
    int waitingTime;

    bool isLive;
    bool dotDam;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    void Start()
    {
        timer = 0.0f;
        waitingTime = 1;
        dotDam = false;
    }

    void Updata()
    {
        timer += Time.deltaTime;

        if (timer > waitingTime) {
            dotDam = true;
            if (dotDam == false)
                timer = 0.0f;
        }
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        if (!isLive)
            return;
        spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true; ;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        anim.SetBool("Slow", false);
        health = maxhealth;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxhealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive || collision.GetComponent<Bullet>().id == 5)
            return;

        if (collision.GetComponent<Bullet>().id == 1 && x == 0) {
            speed -= 1;
            anim.SetBool("Slow", true);
            x++;
        }

        if(collision.GetComponent<Bullet>().id == 7)
        {
            StartCoroutine(DotDam(collision));

        }
        else if (health > 0 && collision.GetComponent<Bullet>().id != 7) {
            Damage(collision);
            StartCoroutine(KnockBack(collision));
            anim.SetTrigger("Hit");
        }
        
        if (health <= 0) {
            isDead();
        }
    }

    IEnumerator KnockBack(Collider2D collision)
    {
        yield return wait;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        if (collision.GetComponent<Bullet>().id != 0) { 
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }
        else {
            rigid.AddForce(dirVec.normalized * 0, ForceMode2D.Impulse);
        }
        
    }

    IEnumerator DotDam(Collider2D collision)
    {
        for (;health > -100000000000000000;) {
            Damage(collision);
            anim.SetTrigger("Hit");
            if (health <= 0) {
                isDead();
                break;
            }
            yield return new WaitForSeconds(1.0f);
            if (health <= 0)
            {
                isDead();
                break;
            }
        }
    }

    void isDead()
    {
        isLive = false;
        coll.enabled = false;
        rigid.simulated = false;
        spriter.sortingOrder = 1;
        anim.SetBool("Dead", true);
        GameManager.instance.kill++;
        GameManager.instance.GetExp();

        if (GameManager.instance.isLive)
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }

    void Damage(Collider2D collision)
    {
        if (GameManager.instance.player.damage <= 3.0f){
            health -= (collision.GetComponent<Bullet>().damage * GameManager.instance.player.damage);
            Debug.Log(collision.GetComponent<Bullet>().damage * GameManager.instance.player.damage);
        }
        else {
            health -= (collision.GetComponent<Bullet>().damage * 3.0f);
            Debug.Log(collision.GetComponent<Bullet>().damage * 3.0f);
        }

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
    }
}
