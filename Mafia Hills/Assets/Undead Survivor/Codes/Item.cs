using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    float sum;
    int a;
    public Weapon weapon;
    public Gear gear;

    Image icon;
    Text textlevel;
    Text textName;
    Text textDesc;

    void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textlevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        textName.text = data.itemName;

    }

    public void OnEnable()
    {
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if(level == 7)
                    textlevel.text = "Lv.Max";
                else
                    textlevel.text = "Lv." + (level + 1);

                if(data.counts[level] == 0 && data.itemId == 1)
                    textDesc.text = "데미지 " + data.damages[level] + " 증가";
                else if(a != 1 && a != 3 & a != 5 && a != 6 && data.itemId == 4)
                    textDesc.text = "데미지 " + data.damages[level] + " 증가";
                else if(data.damages[level] == 0 && data.itemId == 7)
                    textDesc.text = "영역 넓이 증가";
                else if(data.counts[level] == 0 && data.itemId == 9)
                    textDesc.text = "데미지 " + data.damages[level] + " 증가";
                else if(data.counts[level] == 1 && data.itemId == 9)
                    textDesc.text = "붓 개수 " + data.counts[level] + " 증가";
                else
                    textDesc.text = string.Format(data.itemDesc, data.damages[level], data.counts[level]);
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
            case ItemData.ItemType.Power:
            case ItemData.ItemType.Defence:
                sum = 0;

                for(int i = 0; i < level; i++)
                {
                    sum += data.damages[i];
                }

                if (level == 10)
                    textlevel.text = "Lv.Max";
                else
                    textlevel.text = "Lv." + (level);

                if (level == 10)
                {
                    if (data.itemId == 20)
                        textDesc.text = "현재 공격속도 : " + (sum + 1) * 100 + "%";
                    if (data.itemId == 21)
                        textDesc.text = "현재 이동속도 : " + (sum + 1) * 100 + "%";
                    if (data.itemId == 22)
                        textDesc.text = "현재 공격력 : " + (sum + 1) * 100 + "%";
                    if (data.itemId == 23)
                        textDesc.text = "현재 체력 : 600%";
                }
                else
                    textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, (sum + 1) * 100);
                break;
            default:
                textlevel.text = " ";

                textDesc.text = string.Format(data.itemDesc);
                break;
        }
    }

    public void OnClick()
    {
        switch (data.itemType) {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if (level == 0) {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                    if (data.itemId == 6)
                        Weapon.instance.lifeCount++;
                }
                else {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.damages[level];
                    nextCount += data.counts[level];

                    weapon.Levelup(data.damages[level], data.counts[level]);

                    if (data.itemId == 3)
                        GameManager.instance.boomScale += 0.5f;
                    if (data.itemId == 4)
                    {
                        Weapon.instance.scatter++;
                        a++;
                    }
                    if (data.itemId == 7)
                        GameManager.instance.ButterFly();
                }
                level++;
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
            case ItemData.ItemType.Power:
            case ItemData.ItemType.Defence:
                if (level == 0) {
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(data);
                }
                else {
                    float nextRate = data.damages[level];
                    gear.Levelup(nextRate);
                }
                level++;
                break;
            case ItemData.ItemType.Heal:
                GameManager.instance.health = GameManager.instance.maxHealth;
                break;
        }

        if (level == data.damages.Length) {
            GetComponent<Button>().interactable = false;
        }
    }
}
