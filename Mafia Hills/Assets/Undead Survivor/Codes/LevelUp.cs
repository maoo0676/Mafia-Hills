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
        }
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
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

            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                break;
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

    }
    public void GearClick()
    {
        if (count == 0)
        {
            count++;
            GameObject.Find("LevelUp (1)").GetComponent<LevelUp>().Show();
        }
        else
        {
            count--;
            GameObject.Find("LevelUp (1)").GetComponent<LevelUp>().Hide();
        }
    }
}
