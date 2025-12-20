using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "StopAttack", story: "[Self] stop attacking", category: "Action/Custom", id: "fa368537f12376392a820323684ddf71")]
public partial class StopAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<Enemy> Self;
    [SerializeReference] public BlackboardVariable<float> cooldownBeforeNextAttack;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Self.Value.IsEmitterPlaying())
        {
            return Status.Running;
        }
        else
        {
            Self.Value.StopAttack(cooldownBeforeNextAttack);
            return Status.Success;
        }
    }

    protected override void OnEnd()
    {
    }
}

