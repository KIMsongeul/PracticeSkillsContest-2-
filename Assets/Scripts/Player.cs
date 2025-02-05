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
    
    
}
