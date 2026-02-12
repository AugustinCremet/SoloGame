using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class GooState : ActionBaseState
{
    [SerializeField] float _timeBetweenSpawns;
    [SerializeField] float _startTimeBetweenSpawns;

    public GooState(PlayerController playerController, Player player, Animator animator) : base(playerController, player, animator)
    {
        BlockMovement = true;
    }

    public override void EnterState()
    {
        _animator.SetBool("IsGoo", true);
        _animator.Play("ChangeToGoo");
    }

    public override void ExitState()
    {
        _animator.SetBool("IsGoo", false);
    }

    public override void UpdateState()
    {   
    }

    public override void FixedUpdateState()
    {
        if (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "ChangeToGoo")
        {
            return;
        }

        BlockMovement = false;
        _player.HandleGoo();
    }
}
