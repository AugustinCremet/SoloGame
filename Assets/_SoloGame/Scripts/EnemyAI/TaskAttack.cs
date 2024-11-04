using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskAttack : Node
{
    Enemy enemy;
    public TaskAttack(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public override NodeState Evaluate()
    {
        enemy.Attack();   
        state = NodeState.SUCCESS;
        return state;
    }
}
