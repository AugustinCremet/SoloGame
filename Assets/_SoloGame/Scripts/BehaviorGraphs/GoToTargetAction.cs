using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Go to Target", story: "[Agent] go to [Target]", category: "Action/Custom", id: "3300cd977498750242c209721443612f")]
public partial class GoToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    private NavMeshAgent _agent;
    private Transform _targetTrans;
    protected override Status OnStart()
    {
        if(Target == null)
        {
            return Status.Failure;
        }
        _agent = Agent.Value.GetComponent<NavMeshAgent>();
        _targetTrans = Target.Value.transform;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Agent.Value.GetComponent<NavMeshAgent>().SetDestination(Target.Value.transform.position);
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

