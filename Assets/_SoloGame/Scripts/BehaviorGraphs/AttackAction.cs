using System;
using BehaviorTree;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Attack", story: "[Self] attack [Target]", category: "Action/Custom", id: "4e8121d838769b0b13ed7209fabf93ee")]
public partial class AttackAction : Action
{
    [SerializeReference] public BlackboardVariable<Enemy> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    private bool _hasAttacked = false;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (!_hasAttacked)
        {
            Self.Value.StartAttack();
        }

        if (Self.Value.IsEmitterPlaying())
        {
            _hasAttacked = true;
            return Status.Success;
        }
        else
        {
            return Status.Running;
        }
    }

    protected override void OnEnd()
    {
        _hasAttacked = false;
    }
}

