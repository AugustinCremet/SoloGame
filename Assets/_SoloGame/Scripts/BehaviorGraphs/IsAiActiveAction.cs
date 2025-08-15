using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IsAIActive", story: "[Self] is active", category: "Action/Custom", id: "9bd123378903e020f34b042b10caac15")]
public partial class IsAiActiveAction : Action
{
    [SerializeReference] public BlackboardVariable<Enemy> Self;

    protected override Status OnStart()
    {
        Self.Value.SetAI(true);
        return Self.Value.IsAIActive ? Status.Success: Status.Failure;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

