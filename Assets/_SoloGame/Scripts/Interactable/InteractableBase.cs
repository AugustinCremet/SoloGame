using UnityEngine;

public abstract class InteractableBase : MonoBehaviour, IInteractable
{
    [SerializeField] protected SpriteRenderer _popup;

    public void ShowPopup()
    {
        _popup.enabled = true;
    }

    public void HidePopup()
    {
        _popup.enabled = false;
    }

    public abstract void Interact(Player player, PlayerController controller);
}
