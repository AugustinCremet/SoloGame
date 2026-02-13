using UnityEngine;

public enum PipeSide { A, B }
public class PipeEntrance : InteractableBase
{
    [SerializeField] private PipeSide _side;

    private PipeTravel _pipeTravel;

    private void Awake()
    {
        _pipeTravel = GetComponentInParent<PipeTravel>();
        _spriteRenderer.enabled = false;
    }
    public override void Interact(Player player, PlayerController controller)
    {
        _pipeTravel.EnterPipe(transform.position, _side, player, controller);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        _spriteRenderer.enabled = true;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        _spriteRenderer.enabled = false;
    }
}
