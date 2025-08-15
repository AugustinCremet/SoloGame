using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Is taget in range", story: "Is [Target] in range of [Self]", category: "Action/Custom", id: "cf394bfe9a5321385fa08d8392def5c3")]
public partial class IsTagetInRangeAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Self;
    private Transform _targetTrans;

    protected override Status OnStart()
    {
        _targetTrans = Target.Value.transform;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        float distanceToTarget = Vector2.Distance(Self.Value.gameObject.transform.position, _targetTrans.position);
        if (distanceToTarget > Self.Value.stoppingDistance)
        {
            return Status.Failure;
        }

        if (Self.Value.remainingDistance <= Self.Value.stoppingDistance && Self.Value.hasPath)
        {
            return Status.Success;
        }
        else
        {
            return Status.Failure;
        }
    }

    protected override void OnEnd()
    {
    }
}

