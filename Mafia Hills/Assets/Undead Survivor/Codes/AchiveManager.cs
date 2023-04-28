using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{
    public static AchiveManager instance;

    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    public GameObject uiNotice;
    
    enum Achive { UnlockLightSol, UnlockYeonbe, UnlockTwod }
    Achive[] achives;
    WaitForSecondsRealtime wait;

    public bool[] lockSkill = new bool[3];

    void Awake()
    {
        instance = this;

        achives = (Achive[])Enum.GetValues(typeof(Achive));
        wait = new WaitForSecondsRealtime(5);

        if (!PlayerPrefs.HasKey("MyData")){
            Init();
        }
    }

    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);

        foreach (Achive achive in achives) {
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }
    }

    void Start()
    {
        for (int index = 0; index < lockCharacter.Length; index++) {
            lockSkill[index] = false;
        }
        UnlockCharacter();
    }

    void UnlockCharacter()
    {
        for (int index = 0; index < lockCharacter.Length; index++) {
            string achiveName = achives[index].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            if (PlayerPrefs.GetInt(achiveName) == 1) {
                lockSkill[index] = true;
            }
            lockCharacter[index].SetActive(!isUnlock);
            unlockCharacter[index].SetActive(isUnlock);
        }
    }

    void LateUpdate()
    {
        foreach (Achive achive in achives) {
            CheckAchive(achive);
        }
    }

    void CheckAchive(Achive achive) 
    {
        bool isAchive = false;

        switch (achive) {
            case Achive.UnlockLightSol:
                isAchive = GameManager.instance.kill >= 10;
                break;
            case Achive.UnlockYeonbe:
                isAchive = GameManager.instance.kill >= 30;
                break;
            case Achive.UnlockTwod:
                isAchive = GameManager.instance.kill >= 50;
                break;
        }

        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0) {
            PlayerPrefs.SetInt(achive.ToString(), 1);

            for (int index = 0; index < uiNotice.transform.childCount; index++) {
                bool isActive = index == (int)achive;
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());
        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return wait;

        uiNotice.SetActive(false);
    }
}
