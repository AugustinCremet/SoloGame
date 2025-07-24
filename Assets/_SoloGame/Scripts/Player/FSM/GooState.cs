using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class GooState : BaseState
{
    [SerializeField] float _timeBetweenSpawns;
    [SerializeField] float _startTimeBetweenSpawns;

    public GooState(PlayerController playerController, Player player, Animator animator) : base(playerController, player, animator)
    {
        BlockMovement = true;
    }

    public override void EnterState(BaseStateMachine stateMachine)
    {
        _animator.SetBool("IsGoo", true);
        _animator.Play("ChangeToGoo");
    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        _animator.SetBool("IsGoo", false);
    }

    public override void UpdateState(BaseStateMachine stateMachine)
    {   
    }

    public override void FixedUpdateState(BaseStateMachine stateMachine)
    {
        if (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "ChangeToGoo")
        {
            return;
        }

        BlockMovement = false;
        _player.HandleMovement();
        _player.HandleGoo();
    }
}
