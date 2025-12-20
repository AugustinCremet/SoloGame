using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IsAIActiveInChildren", story: "Does [Self] has active AI", category: "Action/Custom", id: "030554ae011790b6fa36ca9af0ca67cc")]
public partial class IsAiActiveInChildrenAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    protected override Status OnStart()
    {
        if(Self.Value.transform.childCount == 0)
        {
            return Status.Failure;
        }
        else
        {
            var enemy = Self.Value.transform.GetChild(0).GetComponent<Enemy>();

            if(enemy.IsAIActive)
            {
                return Status.Success;
            }
            else
            {
                return Status.Failure;
            }
        }
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

