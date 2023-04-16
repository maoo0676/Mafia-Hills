using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Control")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime = 2 * 10f;
    [Header("# Player Info")]
    public int playerId;
    public float health;
    public float maxHealth = 100f;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };
    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject enemyCleaner;
    public GameObject[] Character;

    int x;
    public int a = 0;
    int b; // 실험용
    public float boomScale;

    void Awake()
    {
        instance = this;
        x = Character.Length;
        boomScale = 1.5f;
        b = 0;
    }

    public void GameStart(int id)
    {
        playerId = id;
        health = maxHealth;

        player.gameObject.SetActive(true);
        uiLevelUp.Select(playerId % 10);
        uiLevelUp.Select(10);
        Resume();
    }

    public void GameOver()
    {
        if (Weapon.instance.lifeCount == 0)
        {  
            StartCoroutine(GameOverRoutine());
        }
        else if (Weapon.instance.lifeCount != 0)
        {
            Weapon.instance.lifeCount--;
        }
        
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();
    }

    public void GameVicory()
    {
        StartCoroutine(GameVicoryRoutine());
    }

    IEnumerator GameVicoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(1f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();
    }

    public void GameRetry()
    {
        SceneManager.LoadScene("Tile0Scene");
    }

     public void Update() // 문제있음
    {
        for (int i = 1; i < x; i++)
        {
            if (i + a <= x - 1)
                Character[i + a].SetActive(false);
            else
                Character[(i + a) % 4].SetActive(false);
        }
        Character[a].SetActive(true);

        if (!isLive)
            return;

        if(Input.GetKeyDown(KeyCode.G) && b != 7) { // 실험용
            b++;
            uiLevelUp.Select(playerId % 10);
        }

        if (health > maxHealth)
            health = maxHealth;

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime) {
            gameTime = maxGameTime;
            GameVicory();
        }
    }

    public void GetExp()
    {
        if (!isLive)
            return;

        exp++;

        if (exp == nextExp[Mathf.Min(level, nextExp.Length-1)]) {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }

    public void Left()
    {
        if (a <= 0)
            a = Character.Length - 1;
        else
            a--;
    }

    public void Right()
    {
        if (a >= Character.Length - 1)
            a = 0;
        else
            a++;
    }

    public void ButterFly()
    {
        uiLevelUp.Select(10);
    }
}
