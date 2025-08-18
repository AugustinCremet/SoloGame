using UnityEngine;

public interface IKnockable
{
    public void ApplyKnockback(Vector2 hitLocation, float force);
}
