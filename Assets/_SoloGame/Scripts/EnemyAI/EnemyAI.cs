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
    }
    protected override Node SetupTree()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
        //Node root = new Sequence(new List<Node>
        //{
        //    new TaskCheckIfPlayerOnTheNav(),
        //    new Selector(new List<Node>
        //    {
        //        new Sequence(new List<Node>
        //        {
        //            new TaskCheckPlayerInRange(),
        //            new TaskAttack(),
        //        }),
        //        new Sequence(new List<Node>
        //        {
        //            new TaskStopAttack(),
        //            new TaskGoToTarget(),
        //        }),
        //    }),
        //});
        BehaviorTreeContext context = new BehaviorTreeContext(_enemy, transform, _target.transform, _agent);
        Blackboard bb = new Blackboard();
        bb.Set("ShootTimer", 0f);

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new TaskIsPlayerInRange(),
                new TaskAttack(),
                new TaskResetTimer("ShootTimer"),
            }),
            new Sequence(new List<Node>
            {
                new TaskHasTimerExceeded("ShootTimer", 2f),
                new TaskAttack(),
                new TaskResetTimer("ShootTimer"),
            }),
            new Sequence(new List<Node>
            {
                new TaskStopAttack(),
                new TaskIncrementTimer("ShootTimer"),
                new TaskGoToTarget(),
            }),
        });

        // Basic enemy
        //Node root = new Sequence(new List<Node>
        //{
        //    new TaskMoveBetween(0.15f, 0.25f, 5f),
        //    new TaskAttack(),
        //    new TaskWait(1f),
        //    new TaskStopAttack(),
        //});
        root.SetContext(context);
        root.SetBlackboard(bb);

        return root;
    }
}
