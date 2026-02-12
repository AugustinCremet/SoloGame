using UnityEngine;

public class PlayerStatus
{
    public bool IsDead;
    public bool IsStunned;
    public bool IsInteracting;
    public bool CanMove => !IsDead && !IsStunned && !IsInteracting;
    public bool CanShot => !IsDead && !IsStunned && !IsInteracting;
}
