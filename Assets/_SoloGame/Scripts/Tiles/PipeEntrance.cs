using UnityEngine;

public enum PipeSide { A, B }
public class PipeEntrance : InteractableBase
{
    [SerializeField] private PipeSide _side;
    private PipeTravel _pipeTravel;

    private void Awake()
    {
        _pipeTravel = GetComponentInParent<PipeTravel>();
    }
    public override void Interact(Player player, PlayerController controller)
    {
        _pipeTravel.EnterPipe(transform.position, _side, player, controller);
    }
}
