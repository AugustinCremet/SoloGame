using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IsTargetInSight", story: "[Agent] sees [Target]", category: "Action/Custom", id: "5672048fb09a53767b56ae624f9ff14c")]
public partial class IsTargetInSightAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnStart()
    {
        
        if(Agent == null || Target == null)
        {
            return Status.Failure;
        }
        
        Status status = UtilityFunctions.IsPlayerInSight(Agent.Value.transform.position, Target.Value.transform) ? Status.Success : Status.Failure;
        return status;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

