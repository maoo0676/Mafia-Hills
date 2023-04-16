using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    public Vector2 inputVec;
    public Vector3 playerVec;
    public float speed = 2;
    public float damage = 1;
    public Scanner scanner;
    public RuntimeAnimatorController[] animCon;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake()
    {
        instance = this;
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }

    void OnEnable()
    {
        speed *= Character.Speed;
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    // Update is called once per frame
    
    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        playerVec = GameObject.Find("Player").gameObject.transform.position;
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        anim.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0) {
            spriter.flipX = inputVec.x < 0;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
            return;

        GameManager.instance.health -= Time.deltaTime * 10;

        if (GameManager.instance.health < 0) {

            if(Weapon.instance.lifeCount == 0) { 
                anim.SetTrigger("Dead");

                for (int index = 2; index < transform.childCount; index++) {
                    transform.GetChild(index).gameObject.SetActive(false);
                }
            }
            else {
                GameManager.instance.health = GameManager.instance.maxHealth;
                StartCoroutine(wait());
            }

            GameManager.instance.GameOver();
        }
    }

    IEnumerator wait()
    {
        GameManager.instance.enemyCleaner.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.instance.enemyCleaner.SetActive(false);
    }
}
