using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //private EnemyControl enemyControl;
    public bool isStop = false;

    public int maxHp = 0;
    public int curHp = 0;

    public float speed = 0.1f;
    public LayerMask targetLayer;
    private EnemyControl.EnemyName enemyName;

    public float attackRange = 10.0f;
    public float rotSpeed = 2.0f;

    public float attackDelay = 1.0f;
    public float attackTimer = 0f;

    public int damage = 0;

    public int ex = 0;
    public int score = 0;
    public Player player;
    //public MainControl mainControl;
    public GameObject bulletPrefab;

    private void Start()
    {
        //mainControl = GameObject.Find("MainControl").GetComponent<MainControl>();
    }

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.Find("Player").GetComponent<Player>();
            return;
        }
        Move();
        attackTimer += Time.deltaTime;
    }

    void Move()
    {
        if (Vector3.Distance(transform.position,player.transform.position) > attackRange)
        {
            Vector3 dir = (player.transform.position - transform.position).normalized;
            dir.y = 0;
            transform.Translate(dir * speed * Time.deltaTime ,Space.World);
        }
        else
        {
            AttackEnemy();
        }
    }

    public void SetEnemy(EnemyControl.EnemyName en)
    {
        enemyName = en;
        switch (enemyName)
        {
            case EnemyControl.EnemyName.Bomb:
                maxHp = 20;
                curHp = maxHp;
                speed = 0.2f;
                attackRange = 2.0f;
                damage = 20;
                ex = (int)EnemyControl.EnemyEx.Bomb;
                score= (int)EnemyControl.EnemyScore.Bomb;
                break;
            case EnemyControl.EnemyName.Shoot:
                maxHp = 40;
                curHp = maxHp;
                speed = 0.2f;
                attackRange = 8.0f;
                damage = 5;
                ex = (int)EnemyControl.EnemyEx.Shoot;
                score= (int)EnemyControl.EnemyScore.Shoot;
                break;
            case EnemyControl.EnemyName.Fighter:
                maxHp = 200;
                curHp = maxHp;
                speed = 0.2f;
                attackRange = 2.0f;
                damage = 10;
                ex = (int)EnemyControl.EnemyEx.Fighter;
                score= (int)EnemyControl.EnemyScore.Fighter;
                break;
            case EnemyControl.EnemyName.Boss1:
                maxHp = 200;
                curHp = maxHp;
                speed = 0.2f;
                attackRange = 2.0f;
                damage = 10;
                ex = (int)EnemyControl.EnemyEx.Boss1;
                score= (int)EnemyControl.EnemyScore.Boss1;
                break;
            case EnemyControl.EnemyName.Boss2:
                maxHp = 200;
                curHp = maxHp;
                speed = 0.2f;
                attackRange = 2.0f;
                damage = 10;
                ex = (int)EnemyControl.EnemyEx.Boss2;
                score= (int)EnemyControl.EnemyScore.Boss2;
                break;
            case EnemyControl.EnemyName.Boss3:
                maxHp = 200;
                curHp = maxHp;
                speed = 0.2f;
                attackRange = 2.0f;
                damage = 10;
                ex = (int)EnemyControl.EnemyEx.Boss3;
                score= (int)EnemyControl.EnemyScore.Boss3;
                break;
        }
    }

    public void AttackEnemy()
    {
        if (attackTimer >= attackDelay)
        {
            attackTimer = 0;
            switch (enemyName)
            {
                case EnemyControl.EnemyName.Bomb:
                {
                    player.SetHp(damage);
                    Destroy(gameObject);
                }
                    break;
                case EnemyControl.EnemyName.Shoot:
                {
                    Vector3 dir = new Vector3(player.transform.position.x, 0, player.transform.position.z) -
                                  new Vector3(transform.position.x, 0, transform.position.z);
                    Quaternion targetRot = Quaternion.LookRotation(dir);
                    Bullet bullet = Instantiate(bulletPrefab, transform.position, targetRot).GetComponent<Bullet>();
                    bullet.isEnemy = true;
                    bullet.damage = damage;
                }
                    break;
                case EnemyControl.EnemyName.Fighter:
                {
                    Vector3 dir = new Vector3(player.transform.position.x, 0, player.transform.position.z) -
                                  new Vector3(transform.position.x, 0, transform.position.z);
                    Quaternion targetRot = Quaternion.LookRotation(dir);
                    Bullet bullet = Instantiate(bulletPrefab, transform.position, targetRot).GetComponent<Bullet>();
                    bullet.isEnemy = true;
                    bullet.damage = damage;
                    bullet.transform.GetChild(0).gameObject.SetActive(false);
                }
                    break;
                case EnemyControl.EnemyName.Boss1:
                {
                    StartCoroutine("Cannon");
                }
                    break;
                case EnemyControl.EnemyName.Boss2:
                {
                    
                }
                    break;
                case EnemyControl.EnemyName.Boss3:
                {
                    
                }
                    break;
            }
        }
    }

    private void SetHp(int damage)
    {
        if (!isStop)
        {
            curHp -= damage;
            if (curHp <= 0)
            {
                curHp = 0;
                player.SetEx(ex);
                //mainControl.SetScore(score);
                Destroy(gameObject);
            }
        }
    }
    IEnumerator Cannon()
    {
        
        for (int i = 0; i < 3; i++)
        { 
            yield return new WaitForSeconds(0.01f);
            Vector3 dir = new Vector3(player.transform.position.x, 0, player.transform.position.z) -
                          new Vector3(transform.position.x, 0, transform.position.z);
            Quaternion targetRot = Quaternion.LookRotation(dir);
            Bullet bullet = Instantiate(bulletPrefab, transform.position, targetRot).GetComponent<Bullet>();
            bullet.isEnemy = true;
            bullet.damage = damage / 10;
        }
    }
}
