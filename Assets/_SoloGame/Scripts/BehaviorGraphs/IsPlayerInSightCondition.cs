using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsPlayerInSight", story: "Can [Enemy] see [Target]", category: "Conditions", id: "2b2ea1f084cb25ca867099a36639512e")]
public partial class IsPlayerInSightCondition : Condition
{
    [SerializeReference] public BlackboardVariable<Enemy> Enemy;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    public override bool IsTrue()
    {
        bool isInSight = UtilityFunctions.IsPlayerInSight(Enemy.Value.transform.position, Target.Value.transform);
        return isInSight;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
