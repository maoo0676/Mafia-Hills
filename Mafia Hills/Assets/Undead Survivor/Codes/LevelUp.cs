using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;
    public int count;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    public void Show()
    {
        if (count != 1) { 
            Next();
            AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
            AudioManager.instance.EffectBgm(true);
        }
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
        if (count != 1)
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
            AudioManager.instance.EffectBgm(false);
        }
    }

    public void Select(int index)
    {
        items[index].OnClick();
    }

    void Next()
    {
        foreach (Item item in items) {
            item.gameObject.SetActive(false);
        }

        int[] ran = new int[3];
        while (true) {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);

            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2]
            && ran[0] != 10 && ran[1] != 10 && ran[2] != 10
            && ran[0] != 8 && ran[1] != 8 && ran[2] != 8) {
                if (!AchiveManager.instance.lockSkill[2]
                && !AchiveManager.instance.lockSkill[1]
                && !AchiveManager.instance.lockSkill[0]) {
                    if (ran[0] != 9 && ran[1] != 9 && ran[2] != 9
                    && ran[0] != 7 && ran[1] != 7 && ran[2] != 7
                    && ran[0] != 4 && ran[1] != 4 && ran[2] != 4)
                    break;
                }
                else if (!AchiveManager.instance.lockSkill[2]
                && !AchiveManager.instance.lockSkill[1]) {
                    if (ran[0] != 9 && ran[1] != 9 && ran[2] != 9
                    && ran[0] != 7 && ran[1] != 7 && ran[2] != 7)
                    break;
                }
                else if (!AchiveManager.instance.lockSkill[2]) {
                    if (ran[0] != 9 && ran[1] != 9 && ran[2] != 9)
                    break;
                }
                else
                    break;

            }
            
        }

        for (int index = 0; index < ran.Length; index++) {
            Item ranItem = items[ran[index]];

            if (ranItem.level == ranItem.data.damages.Length) {
                items[8].gameObject.SetActive(true);
            }
            else {
                ranItem.gameObject.SetActive(true);
            }
        }

        Debug.Log(items.Length);
    }
    public void GearClick()
    {
        if (count == 0)
        {
            count++;
            GameObject.Find("GearUp").GetComponent<LevelUp>().Show();
        }
        else
        {
            count--;
            GameObject.Find("GearUp").GetComponent<LevelUp>().Hide();
        }
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }
}
