using UnityEngine;
using BehaviorTree;

public class TaskStartAttackTimer : Node
{

    public override NodeState Evaluate()
    {
        _context.Enemy.StartShootingTimerActive();
        _state = NodeState.SUCCESS;
        Debug.Log("Start Timer");
        return _state;
    }
}
