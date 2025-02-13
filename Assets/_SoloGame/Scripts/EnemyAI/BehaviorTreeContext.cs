using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BehaviorTreeContext : MonoBehaviour
{
    public Enemy Enemy;
    public Transform EnemyTransform;
    public Transform PlayerTransform;
    public NavMeshAgent NavAgent;

    public BehaviorTreeContext(Enemy enemy, Transform enemyTransform, Transform playerTransform, NavMeshAgent navAgent)
    {
        Enemy = enemy;
        EnemyTransform = enemyTransform;
        PlayerTransform = playerTransform;
        NavAgent = navAgent;
    }
}
