using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    
    


}
