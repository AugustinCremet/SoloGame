using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : TreeOfNodes
{
    [SerializeField] GameObject target;
    [SerializeField] LayerMask layerMask;
    public static NavMeshAgent agent;
    Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                //new TaskCheckPlayerInRange(transform, target.transform, layerMask),
                new TaskAttack(enemy),
            }),
            new TaskGoToTarget(target.transform),
        });

        return root;
    }
}
