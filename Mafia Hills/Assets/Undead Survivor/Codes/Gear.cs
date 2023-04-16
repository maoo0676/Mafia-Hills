using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData data)
    {
        name = "Gear " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        type = data.itemType;
        rate = data.damages[0];
        ApplyGear();
    }

    public void Levelup(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    void ApplyGear()
    {
        switch (type) {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                Speedup();
                break;
            case ItemData.ItemType.Power:
                Damageup();
                break;
            case ItemData.ItemType.Defence:
                Healthup();
                break;
        }
    }

    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach(Weapon weapon in weapons) { 
            switch(weapon.id) {
                case 0:
                    float speed = 150 * Character.WeaponSpeed;
                    weapon.speed = speed + (speed * rate);
                    break;
                case 5:
                    break;
                default:
                    speed = 0.5f * Character.WeaponRate;
                    weapon.speed = speed * (1f - rate);
                    break;
            }
        }
    }

    void Speedup()
    {
        float speed = 2 * Character.Speed;
        GameManager.instance.player.speed += speed * rate;

    }

    void Damageup()
    {
        float damage = 1;
        GameManager.instance.player.damage += damage * rate;
        Debug.Log(GameManager.instance.player.damage);

    }

    void Healthup()
    {
        float damage = 100;
        GameManager.instance.maxHealth += damage * rate;
        Debug.Log(GameManager.instance.maxHealth);

    }
}
