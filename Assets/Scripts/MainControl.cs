using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainControl : MonoBehaviour
{
    public int stageNum = 0;
    public int maxStageNum = 2;
    public EnemyControl enemyControl;
    public Player player;
    public int score = 0;

    public GameObject stopUI;
    public GameObject menuUI;
    public Text lvText;
    public Text stopLvText;
    public Text scoreText;
    public Slider hpBar;
    public Slider mpBar;
    public Slider exBar;

    public List<Text> textSkill;

    public List<string> skillName = new List<string>()
    {
        "기관포", "추적탄", "빠른전진", "보호막"
    };

    private void Start()
    {
        enemyControl = GameObject.Find("EnemyControl").GetComponent<EnemyControl>();
        enemyControl.RepeatRespawn();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuUI.activeInHierarchy)
            {
                Time.timeScale = 1;
                menuUI.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                OpenMenuUI();
            }
        }
    }

    public void BtnSkill(int num)
    {
        player.skillLevel[num]++;
        textSkill[num].text = skillName[num] +
                              "\n" + "Lv." + player.skillLevel[num];
        Time.timeScale = 1;
        exBar.value = 0;
        stopUI.SetActive(false);
    }

    public void OpenStopUI()
    {
        stopUI.SetActive(true);
    }
    public void OpenMenuUI()
    {
        menuUI.SetActive(true);
    }

    public void BtnRestart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    public void BtnExit()
    {
        Application.Quit();
    }

    public void SetScore(int num)
    {
        score += num;
        scoreText.text = score.ToString();
    }
}
