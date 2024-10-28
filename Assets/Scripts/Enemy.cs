using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] int hp = 100;

    public void Damage(int dmgAmount)
    {
        hp -= dmgAmount;

        if(hp <= 0)
            Destroy(gameObject);
    }
}
