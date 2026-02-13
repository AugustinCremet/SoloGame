using UnityEngine;

public abstract class InteractableBase : MonoBehaviour, IInteractable
{
    [SerializeField] protected SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
    public void ShowPopup()
    {
        
    }

    public void HidePopup()
    {
        
    }

    public abstract void Interact(Player player, PlayerController controller);
}
