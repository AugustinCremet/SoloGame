using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Android;

public class EnemyAI : TreeOfNodes
{
    [SerializeField] LayerMask _layerMask;
    GameObject _target;
    public static NavMeshAgent Agent;
    Enemy Enemy;

    private void Awake()
    {
        Enemy = GetComponent<Enemy>();
        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
    }
    protected override Node SetupTree()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
        Node root = new Sequence(new List<Node>
        {
            new TaskCheckIfPlayerOnTheNav(_target.transform),
            new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new TaskCheckPlayerInRange(transform, _target.transform, _layerMask),
                    new TaskAttack(Enemy),
                }),
                new Sequence(new List<Node>
                {
                    new TaskStopAttack(Enemy),
                    new TaskGoToTarget(_target.transform),
                }),
            }),
        });

        return root;
    }
}
