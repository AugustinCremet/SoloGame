using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Android;

public class EnemyAI : TreeOfNodes
{
    [SerializeField] GameObject _target;
    [SerializeField] LayerMask _layerMask;
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
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                //TODO removed for testing
                //new TaskCheckPlayerInRange(transform, target.transform, layerMask),
                new TaskAttack(Enemy),
            }),
            new TaskGoToTarget(_target.transform),
        });

        return root;
    }
}
