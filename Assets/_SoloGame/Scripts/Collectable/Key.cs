using UnityEngine;

public class Key : Item, ICollectable, IUniqueIdentifier
{

    private void Start()
    {
        if(GameManager.Instance.IsCollectableCollected(UniqueID))
        {
            Destroy(gameObject);
        }
    }
}
