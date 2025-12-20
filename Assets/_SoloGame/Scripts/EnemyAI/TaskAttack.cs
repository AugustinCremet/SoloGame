using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskAttack : Node
{
    Enemy _enemy;
    public TaskAttack(Enemy enemy)
    {
        this._enemy = enemy;
    }
    public override NodeState Evaluate()
    {
        _enemy.Attack();   
        state = NodeState.SUCCESS;
        return state;
    }
}
