using UnityEngine;

public interface IInteractable
{
    public abstract void Interact(Player player, PlayerController controller);
    public abstract void ShowPopup();
    public abstract void HidePopup();
}
