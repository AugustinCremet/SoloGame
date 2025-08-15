using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Move pass target", story: "[Self] move pass [Target]", category: "Action/Custom", id: "18bfa6758ba2bc2d749fb902dbbb6632")]
public partial class MovePassTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    private Vector2 _initialDirection;
    private Vector2 _lastDesiredPos;
    private Vector2 _desiredPos;
    private bool _stopCalculation = false;

    protected override Status OnStart()
    {
        Vector2 self = Self.Value.transform.position;
        Vector2 target = Target.Value.transform.position;

        _initialDirection = (target - self).normalized;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Vector2 self = Self.Value.transform.position;
        Vector2 target = Target.Value.transform.position;

        // Direction from target to self
        Vector2 diff = self - target;
        

        if(diff.magnitude < 5f && !_stopCalculation)
        {
            Debug.Log("Within distance");
            _stopCalculation = true;
            _desiredPos = _lastDesiredPos;
        }

        if(!_stopCalculation)
        {
            Debug.Log("Not in range");
            Vector2 currentDirection = (target - self).normalized;
            _desiredPos = target + currentDirection * 4f;
            _lastDesiredPos = _desiredPos;
        }


        Self.Value.transform.position = Vector3.MoveTowards(Self.Value.transform.position, _desiredPos, 12f * Time.deltaTime);
        if(Vector2.Distance(_desiredPos, Self.Value.transform.position) <= 0.25f)
        {
            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        _stopCalculation = false;
    }
}

