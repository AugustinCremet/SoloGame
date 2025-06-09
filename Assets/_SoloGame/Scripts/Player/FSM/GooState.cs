using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class GooState : BaseState
{
    [SerializeField] float _timeBetweenSpawns;
    [SerializeField] float _startTimeBetweenSpawns;

    public GooState(PlayerController playerController, Player player, Animator animator) : base(playerController, player, animator)
    {
    }

    public override void EnterState(BaseStateMachine stateMachine)
    {
    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        
    }

    public override void UpdateState(BaseStateMachine stateMachine)
    {
        _playerController.HandleGoo();
    }

    public override void FixedUpdateState(BaseStateMachine stateMachine)
    {
        
    }
}
