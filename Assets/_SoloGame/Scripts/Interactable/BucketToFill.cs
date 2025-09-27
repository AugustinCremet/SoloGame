using UnityEngine;

public class BucketToFill : MonoBehaviour, IInteractable
{
    [SerializeField] string _emptyPhrase;
    [SerializeField] string _fullPhrase;
    public void Interact(Player player)
    {
        player.StartChat(new string[] { _emptyPhrase });
    }
}
