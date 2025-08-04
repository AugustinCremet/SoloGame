using BehaviorTree;
using UnityEngine;

public class TaskCanAttack : Node
{
    public override NodeState Evaluate()
    {
        _state = _context.Enemy.IsAttackCooldown() ? NodeState.FAILURE : NodeState.SUCCESS;
        return _state;
    }
}
