using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class TaskStopAttack : Node
{
    Enemy _enemy;
    public TaskStopAttack(Enemy enemy)
    {
        this._enemy = enemy;
    }
    public override NodeState Evaluate()
    {
        _enemy.StopAttack();
        state = NodeState.SUCCESS;
        return state;
    }
}
