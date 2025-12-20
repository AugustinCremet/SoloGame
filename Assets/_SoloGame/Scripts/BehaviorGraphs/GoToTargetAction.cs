using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Go to Target", story: "[Self] go to [Target]", category: "Action/Custom", id: "3300cd977498750242c209721443612f")]
public partial class GoToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    private Transform _targetTrans;
    private Vector3 _currentDestination;
    protected override Status OnStart()
    {
        if(Target == null)
        {
            return Status.Failure;
        }
        _targetTrans = Target.Value.transform;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        // Prevent movement during a bullet pattern
        if (Self.Value.gameObject.GetComponent<Enemy>().IsEmitterPlaying())
        {
            Self.Value.isStopped = true;
        }

        if (Self.Value.remainingDistance > Self.Value.stoppingDistance || _currentDestination == Vector3.zero)
        {
            // Make sure the bullet pattern is done before moving
            if(!Self.Value.gameObject.GetComponent<Enemy>().IsEmitterPlaying())
            {
                Self.Value.isStopped = false;
                _currentDestination = _targetTrans.position;
                Self.Value.SetDestination(_targetTrans.position);
            }
        }

        if (!Self.Value.pathPending && Self.Value.remainingDistance <= Self.Value.stoppingDistance)
        {
            _currentDestination = Vector3.zero;
            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

