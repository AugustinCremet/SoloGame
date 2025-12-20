using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void Damage(int dmgAmount, Vector2? hitLocation = null, float force = 0f);
}
