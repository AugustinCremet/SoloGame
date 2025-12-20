using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Can attack", story: "Can [Self] attack", category: "Action/Custom", id: "fb749902f34aa814f8ec44c9d7718903")]
public partial class CanAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<Enemy> Self;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Self.Value.IsAttackCooldown() ? Status.Running : Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

