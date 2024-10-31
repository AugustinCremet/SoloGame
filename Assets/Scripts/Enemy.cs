using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] int hp = 100;
    [SerializeField] GameObject bullet;
    string tagSelf;
    float currentWaitingTime = 0f;
    bool canAttack = false;

    private void Awake()
    {
        tagSelf = gameObject.tag;
    }

    private void Update()
    {
        if(!canAttack)
        {
            currentWaitingTime += Time.deltaTime;
            if (currentWaitingTime > 3f)
            {
                canAttack = true;
                currentWaitingTime = 0f;
            }
        }
    }
    public void Damage(int dmgAmount)
    {
        hp -= dmgAmount;

        if(hp <= 0)
            Destroy(gameObject);
    }

    public void Attack()
    {
        if (canAttack)
        {
            canAttack = false;
            GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            newBullet.GetComponent<Bullet>().tagToIgnore = tagSelf;
        }
            
    }
}
