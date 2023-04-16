using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartFlip : MonoBehaviour
{
    void Update()
    {
        if (Weapon.instance.lifeCount != 0)
            gameObject.SetActive(true);
        else if (Weapon.instance.lifeCount == 0)
            gameObject.SetActive(false);
    }

    void FixedUpdate()
    {

        if (!GameManager.instance.isLive)
            return;

        if (Player.instance.inputVec.x != 0)
        {
            if (Bullet.instance.id == 6)
                Bullet.instance.spriter.flipX = Player.instance.inputVec.x < 0;
        }
    }
}
