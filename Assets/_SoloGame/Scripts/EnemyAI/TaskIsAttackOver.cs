using BehaviorTree;
using UnityEngine;

public class TaskIsAttackOver : Node
{
    public override NodeState Evaluate()
    {
        if(_context.Enemy.IsEmitterPlaying())
        {
            _state = NodeState.RUNNING;
        }
        else
        {
            _context.Enemy.StopAttack(3f);
            _state = NodeState.SUCCESS;
        }

        return _state;  
    }
}
