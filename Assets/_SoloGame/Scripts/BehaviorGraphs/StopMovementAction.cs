using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Stop movement", story: "Stop [self]", category: "Action/Custom", id: "db9c975795f8b842881624e83807d406")]
public partial class StopMovementAction : Action
{
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Self;

    protected override Status OnStart()
    {
        Self.Value.isStopped = true;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

