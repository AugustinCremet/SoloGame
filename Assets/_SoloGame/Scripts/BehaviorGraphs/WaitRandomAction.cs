using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Wait random", story: "Wait between [min] and [max] seconds", category: "Action/Custom", id: "8032eed57fb8747ca41392abfa8f7b05")]
public partial class WaitRandomAction : Action
{
    [SerializeReference] public BlackboardVariable<float> Min;
    [SerializeReference] public BlackboardVariable<float> Max;
    private float _timer;

    protected override Status OnStart()
    {
        _timer = UnityEngine.Random.Range(Min.Value, Max.Value);

        if (_timer <= 0.0f)
        {
            return Status.Success;
        }

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

