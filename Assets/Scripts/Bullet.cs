using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10.0f;
    public float endTime = 2.0f;
    public float lifeTime = 0f;
    public int damage = 1;
    public Transform enemy = null;

    public bool isEnemy = false;
    private Player player = null;


    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void Update()
    {
        lifeTime += Time.deltaTime;
        if (enemy == null)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        else
        {
            Vector3 dir = (enemy.position - transform.position).normalized;
            dir.y = 0;
            transform.Translate(dir * speed * Time.deltaTime,Space.World);
        }

        if (lifeTime >= endTime)
        {
            lifeTime = 0;
            Destroy(gameObject);
        }
    }

    public void SetEnemy(Transform targetEnemy)
    {
        enemy = targetEnemy;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isEnemy)
        {
            if (other.CompareTag("Enemy"))
            {
                Enemy enemy = other.GetComponent<Enemy>();
                enemy.SetHp(damage);
                Destroy(gameObject);
            }
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                player.SetHp(damage);
                Destroy(gameObject);
            }
        }
    }
}
