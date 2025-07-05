using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class GooState : BaseState
{
    [SerializeField] float _timeBetweenSpawns;
    [SerializeField] float _startTimeBetweenSpawns;
    private SpriteRenderer _eyeSprite;
    private Animator _eyeAnimator;

    public GooState(PlayerController playerController, Player player, Animator animator, GameObject eyeAnimator) : base(playerController, player, animator)
    {
        _eyeAnimator = eyeAnimator.GetComponent<Animator>();
        _eyeSprite = eyeAnimator.GetComponent<SpriteRenderer>();
        BlockMovement = true;
    }

    public override void EnterState(BaseStateMachine stateMachine)
    {
        _eyeSprite.enabled = true;
        _animator.SetBool("IsGoo", true);
        _animator.Play("ChangeToGoo");
    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        _animator.SetBool("IsGoo", false);
        _eyeSprite.enabled = false;
    }

    public override void UpdateState(BaseStateMachine stateMachine)
    {
        _eyeAnimator.SetFloat("Horizontal", _playerController.MovementVector.x);
        _eyeAnimator.SetFloat("Vertical", _playerController.MovementVector.y);       
    }

    public override void FixedUpdateState(BaseStateMachine stateMachine)
    {
        if (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "ChangeToGoo")
        {
            return;
        }

        BlockMovement = false;
        _playerController.HandleMovement();
        _playerController.HandleGoo();
    }
}
