using UnityEngine;

public class Chest : InteractableBase
{
    [SerializeField] Sprite _openedChestSprite;
    [SerializeField] Sprite _closedChestSprite;
    private bool _isOpened = false;

    public override void Interact(Player player, PlayerController controller)
    {
        if(!_isOpened)
        {
            OpenChest();
            Debug.Log("Chest opened! You found some treasure!");
        }
        else
        {
            Debug.Log("The chest is already opened.");
        }
    }

    private void OpenChest()
    {
        _isOpened = true;
        _spriteRenderer.sprite = _openedChestSprite;
    }
}
