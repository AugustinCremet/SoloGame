using UnityEngine;

public class Key : MonoBehaviour, ICollectable
{
    public void OnCollect(Player player)
    {
        player.GiveKey();
        Destroy(gameObject);
    }
}
