using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public enum EnemyName
    {
        Bomb,
        Shoot,
        Fighter,
        Boss1,
        Boss2,
        Boss3
    }
    public enum EnemyScore
    {
        Bomb = 10,
        Shoot = 10,
        Fighter = 20,
        Boss1 = 500,
        Boss2 = 1000,
        Boss3 = 2000
    }
    public enum EnemyEx
    {
        Bomb = 10,
        Shoot = 20,
        Fighter = 30,
        Boss1 = 500,
        Boss2 = 500,
        Boss3 = 500
    }
    public enum StageEnemyNum
    {
        Stage01 = 50,
        Stage02 = 100,
        Stage03 = 150
    }

    public MainControl mainControl;
    public GameObject enemyPrefab;
    public int addEnemyNum = 0;

    public void RepeatRespawn()
    {
        InvokeRepeating("Respawn", 3.0f,3.0f);
    }

    void Respawn()
    {
        switch (mainControl.stageNum)
        {
            case 0:
                if (addEnemyNum < (int)StageEnemyNum.Stage01)
                {
                    Instantiate(enemyPrefab, new Vector3(Random.Range(-80, 80), 0, 50), enemyPrefab.transform.rotation);
                    addEnemyNum++;
                }
                else
                {
                    CancelInvoke("Respawn");
                }
                break;
            case 1:
                if (addEnemyNum < (int)StageEnemyNum.Stage02)
                {
                    float tempX = Random.Range(-80, 80);
                    int enemyNum = Random.Range(0, 2);
                    if (tempX > -50 && tempX < 50)
                    {
                        Enemy enemy = Instantiate(enemyPrefab, new Vector3(tempX,0,50),enemyPrefab.transform.rotation).GetComponent<Enemy>();
                        enemy.SetEnemy((EnemyName)enemyNum);
                    }
                    else
                    {
                        Enemy enemy = Instantiate(enemyPrefab, new Vector3(tempX, 0,Random.Range(-80, 80)),
                            enemyPrefab.transform.rotation).GetComponent<Enemy>();
                        enemy.SetEnemy((EnemyName)enemyNum);
                    }
                    addEnemyNum++;
                }
                else
                {
                    CancelInvoke("Respawn");
                }
                break;
            case 2:
                if (addEnemyNum < (int)StageEnemyNum.Stage03)
                {
                    float tempX = Random.Range(-80, 80);
                    int enemyNum = Random.Range(0, 3);
                    if (tempX > -50 && tempX < 50)
                    {
                        int minus = 1;
                        if (Random.Range(0,2) == 1)
                        {
                            minus = -1;
                        }
                        Enemy enemy = Instantiate(enemyPrefab, new Vector3(tempX,0,50 * minus),enemyPrefab.transform.rotation).GetComponent<Enemy>();
                        enemy.SetEnemy((EnemyName)enemyNum);
                    }
                    else
                    {
                        Enemy enemy = Instantiate(enemyPrefab, new Vector3(tempX, 0,Random.Range(-80, 80)),
                            enemyPrefab.transform.rotation).GetComponent<Enemy>();
                        enemy.SetEnemy((EnemyName)enemyNum);
                    }
                    addEnemyNum++;
                }
                else
                {
                    CancelInvoke("Respawn");
                }
                break;
        }
    }
}
