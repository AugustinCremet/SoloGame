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
    NavMeshAgent _agent;
    Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.radius = 0.5f;
    }
    protected override Node SetupTree()
    {
        _target = GameObject.FindGameObjectWithTag("Player");

        BehaviorTreeContext context = new BehaviorTreeContext(_enemy, transform, _target.transform, _agent);

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new TaskIsPlayerInRange(),
                new TaskStopMovement(),
                new TaskCanAttack(),
                new TaskAttack(),
                new TaskIsAttackOver(),
            }),
            new Sequence(new List<Node>
            {
                new TaskHasTimerExceeded(5f),
                new TaskStopMovement(),
                new TaskCanAttack(),
                new TaskAttack(),
                new TaskIsAttackOver(),
            }),
            new Sequence(new List<Node>
            {
                new TaskStartAttackTimer(),
                new TaskGoToTarget(),
            }),
        });

        root.SetContext(context);

        return root;
    }
}
