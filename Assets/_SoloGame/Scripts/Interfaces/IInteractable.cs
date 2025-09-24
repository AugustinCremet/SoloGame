using UnityEngine;

public interface IInteractable
{
    public abstract void Interact(Player player);
    public virtual string GetPrompt() => "Interact";
}
