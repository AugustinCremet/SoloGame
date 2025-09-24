using UnityEngine;

public class BucketToFill : MonoBehaviour, IInteractable
{
    public void Interact(Player player)
    {
        player.gameObject.GetComponentInChildren<ChatBubble>().SetText("Oh an empty bucket");
    }
}
