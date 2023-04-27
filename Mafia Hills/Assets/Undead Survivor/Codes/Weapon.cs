using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public static Weapon instance;

    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;
    public float skillSpeed;
    public int lifeCount;
    public int scatter;
    int x = 0;
    int xx = 0;
    int massage = 0;

    float timer;
    float skillTimer;
    Player player;
    Vector3 scale = Vector3.zero;

    void Awake()
    {
        instance = this;
        player = GameManager.instance.player;
        lifeCount = 0;
        scatter = 0;
        switch (GameManager.instance.a)
        {
            case 0:
                skillSpeed = 6f;
                break;
            case 1:
                skillSpeed = 6f;
                break;
            case 2:
                skillSpeed = 6f;
                break;
            case 3:
                skillSpeed = 6f;
                break;
        }
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            case 1 :
                timer += Time.deltaTime;

                if (timer > speed) {
                    timer = 0f;
                    Fire();
                }
                break;
            case 4:
                if(Weapon.instance.scatter > scatter) {
                    scatter = Weapon.instance.scatter;
                }
                timer += Time.deltaTime;

                if (timer > speed) {
                    timer = 0f;
                    Fire();
                }
                break;
            case 5:
                timer += Time.deltaTime;

                if (timer > speed) { 
                    timer = 0f;
                    Batch();
                }
                break;
            case 6:
                timer += Time.deltaTime;
                
                if(Weapon.instance.lifeCount < lifeCount && x == 0) {
                    Weapon.instance.lifeCount = lifeCount;
                    x++;
                }
                else if (Weapon.instance.lifeCount < lifeCount && x != 0) {
                    lifeCount = Weapon.instance.lifeCount;
                    x--;
                }
                else if (Weapon.instance.lifeCount > lifeCount) {
                    Weapon.instance.lifeCount = lifeCount;
                }

                if (timer > this.damage) {
                    if(lifeCount == 0)
                        timer = 0f;
                        lifeCount++;
                }
                break;
            case 7:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            case 9:
                timer += Time.deltaTime;

                if (timer > speed) { 
                    timer = 0f;
                    Batch();
                }
                break;
            case 10:

                break;
            default:
                timer += Time.deltaTime;

                if (timer > speed) {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        skillTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Q)) {
            if (skillTimer > skillSpeed) {
                massage--;
                switch (GameManager.instance.a)
                {
                    case 0:
                        KevinSkill();
                        skillTimer = 0f;
                        
                        break;
                    case 1:
                        LightSolSkill();
                        skillTimer = 0f;
                        
                        break;
                    case 2:
                        YeonbeSkill();
                        skillTimer = 0f;
                        
                        break;
                    case 3:
                        TwodSkill();
                        skillTimer = 0f;
                        
                        break;
                }

                AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
            }
            else
                Debug.Log("재사용 대기 시간 : " + (skillSpeed - skillTimer));
        }

        if(skillTimer > skillSpeed && massage == 0) {
            Debug.Log("재사용 대기 시간 : 0");
            massage++;
        }
    }

    public void Levelup(float damage, int count)
    {
        this.damage += damage * Character.Damage;
        this.count += count;

        if (id == 0)
            Batch();
        if (id == 5 || id == 3 || id == 6)
            this.count -= count;
        if (id == 7) {
            this.damage -= damage * Character.Damage;
            this.count -= count;
            scale.x = 0.25f;
            scale.y = 0.25f;
            Batch();
        }
        if (id == 9) {
            Batch();
        }

        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Weapon" + data.itemId;
        transform.parent = player.transform;
        if (id == 0 || id == 7) { 
            transform.localPosition = Vector3.up * -0.45f;
        }
        else {
            transform.localPosition = Vector3.zero;
        }

        //Property Set
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        for (int index = 0; index < GameManager.instance.pool.prefabs.Length; index++) { 
            if (data.projectile == GameManager.instance.pool.prefabs[index]) {
                prefabId = index;
                break;
            }
        }

        switch (id) {
            case 0:
                speed = 150 * Character.WeaponSpeed;
                Batch();
                break;
            case 1:
                speed = 1.0f * Character.WeaponRate;
                break;
            case 2:
                speed = 2.6f * Character.WeaponRate;
                break;
            case 3 :
                speed = 2.1f * Character.WeaponRate;
                break;
            case 4:
                speed = 1.4f * Character.WeaponRate;
                break;
            case 5:
                speed = 5.0f;
                break;
            case 6:
                speed = 0f;
                Batch();
                break;
            case 7:
                speed = 0;
                Batch();
                break;
            case 9:
                speed = 2.0f;
                break;
            default:

                break;
        }

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Batch()
    {
        for (int index = 0; index < count; index++) {
            Transform bullet; 

            if (index < transform.childCount) {
                bullet = transform.GetChild(index);
            }
            else { 
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;   
            }
            
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            if (id == 0) { 
                Vector3 rotVec = Vector3.forward * 360 * index / count;
                bullet.Rotate(rotVec);
                bullet.Translate(bullet.up * 3.5f, Space.World);
                bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero);// -100 is Infinity per.
            }
            else if (id == 7) {
                bullet.localScale += scale;
                bullet.Translate(bullet.up * 0f, Space.World);
                bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero);
            }
            else if (id == 5) {
                bullet.Translate(bullet.up * 2.5f, Space.World);
                bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero);
            }
            else if (id == 6) {
                bullet.Translate(bullet.right * -0.6f, Space.World);
                bullet.Translate(bullet.up * 0.45f, Space.World);
                bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero);
            }
            else if (id == 9) {
                bullet.Translate(bullet.up * 0.7f, Space.World);
                bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero);
            }
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;
        
        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 targetPos1 = targetPos + new Vector3(1, 0, 0);
        Vector3 targetPosA = targetPos + new Vector3(0.5f, 0, 0);
        Vector3 targetPos2 = targetPos + new Vector3(-1, 0, 0);
        Vector3 targetPosB = targetPos + new Vector3(-0.5f, 0, 0);
        Vector3 targetPos3 = targetPos + new Vector3(2, 0, 0);
        Vector3 targetPosC = targetPos + new Vector3(1.5f, 0, 0);
        Vector3 targetPos4 = targetPos + new Vector3(-2, 0, 0);
        Vector3 targetPosD = targetPos + new Vector3(-1.5f, 0, 0);

        Vector3 dir = targetPos - transform.position;
        Vector3 dir1 = targetPos1 - transform.position;
        Vector3 dirA = targetPosA - transform.position;
        Vector3 dir2 = targetPos2 - transform.position;
        Vector3 dirB = targetPosB - transform.position;
        Vector3 dir3 = targetPos3 - transform.position;
        Vector3 dirC = targetPosC - transform.position;
        Vector3 dir4 = targetPos4 - transform.position;
        Vector3 dirD = targetPosD - transform.position;

        dir = dir.normalized;
        dir1 = dir1.normalized;
        dirA = dirA.normalized;
        dir2 = dir2.normalized;
        dirB = dirB.normalized;
        dir3 = dir3.normalized;
        dirC = dirC.normalized;
        dir4 = dir4.normalized;
        dirD = dirD.normalized;

        if (id == 4 && scatter <= 0)
        {
            Firefire(dir, 0);
        }
        else if (id == 4 && scatter <= 2)
        {
            Firefire(dirA, 0);

            Firefire(dirB, 1);
        }
        else if (id == 4 && scatter <= 4)
        {
            Firefire(dir, 0);

            Firefire(dir1, 1);

            Firefire(dir2, 1);
        }
        else if (id == 4 && scatter <= 6)
        {
            Firefire(dirA, 0);

            Firefire(dirB, 1);

            Firefire(dirC, 1);

            Firefire(dirD, 1);
        }
        else if (id == 4 && scatter == 7)
        {
            Firefire(dir, 0);

            Firefire(dir1, 1);

            Firefire(dir2, 1);

            Firefire(dir3, 1);

            Firefire(dir4, 1);
        }
        else
        {
            Firefire(dir, 0);
        }
    }

    void Firefire(Vector3 dir, int a)
    {
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        if (a == 0)
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }

    void Swing()
    {
        
        for (int index = 0; index < count; index++) {
            Transform bullet; 

            if (index < transform.childCount) {
                bullet = transform.GetChild(index);
            }
            else { 
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;   
            }
            
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;
            
            bullet.Translate(bullet.up * 0.7f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero);// -100 is Infinity per.
        }
    }

    void KevinSkill()
    {
        if (id != 3)
            return;
        
        float[] X = { 0, 2.5f, 5, 5, 5, 5, 5, 2.5f, 0, -2.5f, -5, -5, -5, -5, -5, -2.5f };
        float[] Y = { 5, 5, 5, 2.5f, 0, -2.5f, -5, -5, -5, -5, -5, -2.5f, 0, 2.5f, 5, 5 };

        Vector3 dir = new Vector3(0, 0, 0);

        for(int i = 0; i < 16; i++) {
            Vector3 dir01 = new Vector3(X[i], Y[i], 0);

            dir01 = dir01.normalized;
            
            Firefire(dir01, 1);
        }

        StartCoroutine(waiting(0, dir));
    }

    void LightSolSkill()
    {
        if (id != 4)
            return;

        xx = 0;

        for (int i = 0; i < 40; i++){
            float r1 = Random.Range(-5, 5);
            float r2 = Random.Range(-5, 5);
            if(r1 == 0 && r2 == 0)
                r1 = 1;

            Vector3 dir = new Vector3(r1, r2, 0);

            dir = dir.normalized;

            StartCoroutine(waiting(1, dir));

            xx++;
        }
    }

    void YeonbeSkill()
    {
        if(id != 10)
            return;

        for (int i = 0; i < 5; i++){
            float r1 = Random.Range(-5, 5);
            float r2 = Random.Range(-5, 5);
            if(r1 == 0 && r2 == 0)
                r1 = 1;

            Vector3 dir = new Vector3(r1, r2, 0);

            dir = dir.normalized;

            Firefire(dir, 1);
        }
    }

    void TwodSkill()
    {

    }

    IEnumerator waiting(int a, Vector3 dir)
    {
        switch(a) {
            case 0:
                Debug.Log("빨라짐");
                GameManager.instance.player.speed += 2f;

                yield return new WaitForSeconds(2.0f);

                Debug.Log("느려짐");
                GameManager.instance.player.speed -= 2f;
            break;
            case 1:
                yield return new WaitForSeconds(0.025f * xx);

                Firefire(dir, 1);
            break;
        }
    }
}