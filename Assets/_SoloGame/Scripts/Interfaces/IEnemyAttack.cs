using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAttack
{
    void StartAttack();
    bool IsAttacking { get; }
}
