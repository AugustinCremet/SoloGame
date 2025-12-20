using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Initialize context", story: "Set context for [Self]", category: "Action/Custom", id: "4a7d7b5871c3fecfea491e3abe75fe83")]
public partial class InitializeContextAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    private bool _isInitDone = false;
    protected override Status OnStart()
    {
        if(Self != null && !_isInitDone)
        {
            _isInitDone = true;
            var agent = Self.Value.GetComponent<BehaviorGraphAgent>();
            var player = GameObject.FindGameObjectWithTag("Player");
            agent.SetVariableValue("Target", player);

            if (Self.Value.TryGetComponent<Enemy>(out var enemy))
            {
                agent.SetVariableValue("Enemy", enemy);
            }
        }

        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

