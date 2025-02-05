using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int level = 0;
    public int maxLevel = 5;
    public int maxEx = 0;
    public int curEx = 0;

    private List<int> ExNum = new List<int>()
    {
        0, 100, 200, 300, 500, 800, 1200
    };

    public int maxHp = 100;
    public int hp = 100;

    public int shieldHp = 0;
    public int maxShieldHp = 200;
    public float shieldTime = 0;

    public int maxMp = 50;
    public int mp = 50;

    public float baseMoveSpeed = 5.0f;
    public float moveSpeed = 5.0f;
    public float rotSpeed = 90.0f;
    
    public float attackRange = 10.0f;
    public float attackDelay = 1.0f;
    public int attackDamage = 3;
    private float attackTimer = 0f;
    

    public Transform turret;
    public Transform firePoint;
    public float turretTurnSpeed = 50.0f;
    public float turretPitchSpeed = 30.0f;

    public GameObject bulletPrefab;
    public GameObject cannonPrefab;
    public GameObject tracerPrefab;

    public float detectionRange = 10.0f;

    public LayerMask enemyLayer;

    public float minRot = 5.0f;

    private bool isTarget = false;

    public Transform targetEnemy = null;

    private List<bool> isSkill = new List<bool>()
    {
        false, false, false, false
    };

    enum SkillName
    {
        cannon = 0,
        tracer = 1,
        fast = 2,
        shield = 3
    }

    private List<float> skillDelay = new List<float>()
    {
        0, 2.0f, 10.0f, 5.0f, 3.0f
    };
    private List<int> skillConsume = new List<int>()
    {
        1,10,5,20
    };

    private List<int> skillLevel = new List<int>()
    {
        0, 0, 0, 0
    };

    //public MessageBox messagebox;
    private bool isStop = false;
    //private MainControl mainControl;
    
    private Rigidbody rigid;


    private void Start()
    {
        //mainControl = GameObject.Find("MainControl").GetComponent<MainControl>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        TurnMove();
        Attack();
        if (shieldTime > 0)
        {
            shieldTime -= Time.deltaTime;
        }
    }

    void TurnMove()
    {
        float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float turn = Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime;
        
        rigid.MovePosition(rigid.position + (transform.forward.normalized * move));
        transform.Rotate(0,turn,0);

        FindTarget();
        if (targetEnemy != null)
        {
            RotateTurret();
        }
    }

    void FindTarget()
    {
        Collider[] hitcolliders = Physics.OverlapSphere(transform.position, detectionRange, enemyLayer);
        if (hitcolliders.Length > 0)
        {
            float saveDistance = Mathf.Infinity;
            foreach (var hit in hitcolliders)
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < saveDistance)
                {
                    saveDistance = distance;
                    targetEnemy = hit.transform;
                }
            }
        }
        else
        {
            targetEnemy = null;
        }
    }

    void RotateTurret()
    {
        Vector3 dir = targetEnemy.position - turret.position;
        dir.y = 0;
        Quaternion targetRot = Quaternion.LookRotation(dir);
        
        turret.rotation = Quaternion.Lerp(turret.rotation,targetRot,Time.deltaTime * rotSpeed);
        if (Vector3.Angle(turret.forward, dir) < minRot)
        {
            isTarget = true;
            if (attackTimer >= attackDelay)
            {
                attackTimer = 0;
                Shoot();
            }
        }
        else
        {
            isTarget = false;
        }
    }
    void Attack()
    {
        if (isTarget)
        {
            attackTimer = Time.deltaTime;
        }
    }

    void Shoot()
    {
        Vector3 dir = new Vector3(targetEnemy.position.x, 0, targetEnemy.position.z) -
                      new Vector3(firePoint.position.x, 0, firePoint.position.z);
        Quaternion targetRot = Quaternion.LookRotation(dir);
        //Bullet bullet = Instantiate(bulletPrefab, firePoint.position, targetRot).GetComponent<Bullet>();
        //bullet.damage = attackDamage;
    }

    public void UseSkill(int skillName)
    {
        if (skillLevel[skillName] > 0)
        {
            if (!isSkill[skillName])
            {
                if (mp >= skillConsume[skillName])
                {
                    isSkill[skillName] = true;
                    switch (skillName)
                    {
                        case (int)SkillName.cannon:
                            StartCoroutine("Cannon");
                            break;
                        case (int)SkillName.tracer:
                            StartCoroutine("tracer");
                            break;
                        case (int)SkillName.fast:
                            StartCoroutine("fast");
                            break;
                        case (int)SkillName.shield:
                            StartCoroutine("shield");
                            break;
                    }
                }
                else
                {
                    //messageBox.ShowMessageBox("마나가 부족합니다.");
                }
            }
            else
            {
                //messageBox.ShowMessageBox("스킬을 아직 사용할 수 없습니다.");
            }
        }
        else
        {
            //messageBox.ShowMessageBox("스킬을 아직 획득하지 않았습니다.");
        }
    }

    IEnumerator Cannon()
    {
        int skillNum = (int)SkillName.cannon;

        if (!targetEnemy)
        {
            //messageBox.ShowMessageBox("공격할 수 있는 적이 없습니다.");
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(0.2f);
                Vector3 dir = new Vector3(targetEnemy.position.x, 0, targetEnemy.position.z) -
                              new Vector3(firePoint.position.x, 0, firePoint.position.z);
                Quaternion targetRot = Quaternion.LookRotation(dir);
                //Bullet bullet = Instantiate(cannonPrefab, firePoint.position, targetRot).GetComponent<Bullet>();
                //bullet.damage = attackDamage / 3;
                mp = mp - skillConsume[skillNum];
                //mainControl.mpBar.value = (float)mp / maxHp;
            }
        }
        yield return new WaitForSeconds(skillDelay[skillNum]);
        isSkill[skillNum] = false;
    }
    IEnumerator Tracer()
    {
        int skillNum = (int)SkillName.tracer;

        if (!targetEnemy)
        {
            //messageBox.ShowMessageBox("공격할 수 있는 적이 없습니다.");
        }
        else
        {
            //Bullet.bullet = Instantiate(tracerPrefab, firePoint.position, tracerPrefab.transform.position).GetComponent<Bullet>();
            //bullet.damage = attackDamage;
            //bullet.SetEnemy(targetEnemy);
            mp = mp - skillConsume[skillNum];
            //mainControl.mpBar.value = (float)mp / maxHp;
        }
        yield return new WaitForSeconds(skillDelay[skillNum]);
        isSkill[skillNum] = false;
    }
    IEnumerator Fast()
    {
        int skillNum = (int)SkillName.fast;

        moveSpeed *= 1.4f;
        mp = mp - skillConsume[skillNum];
        //mainControl.mpBar.value = (float)mp / maxHp;
        yield return new WaitForSeconds(skillDelay[skillNum]);
        moveSpeed = baseMoveSpeed;
        isSkill[skillNum] = false;
    }
    IEnumerator Shield()
    {
        int skillNum = (int)SkillName.shield;

        shieldHp = maxHp;
        shieldTime = 5.0f;
        mp = mp - skillConsume[skillNum];
        //mainControl.mpBar.value = (float)mp / maxHp;
        yield return new WaitForSeconds(skillDelay[skillNum]);
        isSkill[skillNum] = false;
        yield return new WaitUntil(() => shieldTime <= 0);
        shieldHp = 0;
    }

    public void SetHp(int damage)
    {
        if (!isStop)
        {
            if (shieldHp > 0)
            {
                shieldHp -= damage;
            }
            else
            {
                hp -= damage;
                //mainControl.hpBar.value = (float)hp / maxHp;

                if (hp <= 0)
                {
                    //mainControl.hpBar.value = 0;
                    hp = 0;
                    Time.timeScale = 0;
                    //mainControl.OpenMenuUI();
                }
                
            }
        }
    }

    public void SetEx(int ex)
    {
        curEx += ex;
        //mainControl.exBar.value = (float)curEx / maxEx;
        if (curEx > maxEx)
        {
            if (level < ExNum.Count)
            {
                level++;
                curEx = 0;
                maxEx = ExNum[level];
                Time.timeScale = 0;
                maxHp = (int)(maxHp + (maxHp * 0.1f));
                hp = maxHp;
                maxMp = (int)(maxMp + (maxMp * 0.1f));
                mp = maxMp;
                attackDamage = (int)(attackDamage + (attackDamage * 0.1f));

                // mainControl.LvText.text = "Lv." + level;
                // mainControl.StopLvText.text = "Lv." + level;
                // mainControl.hpBar.value = 1;
                // mainControl.mpBar.value = 1;
                // mainControl.OpenStopUI();

            }
        }
    }
    
}
