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

    new private void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    protected override Node SetupTree()
    {
        Node root = new Sequence(new List<Node>
        {
            new CheckPlayerInRange(transform, target.transform, layerMask),
            new TaskGoToTarget(target.transform),
        });

        return root;
    }
}
