using UnityEngine;

public class Key : MonoBehaviour, ICollectable, IUniqueIdentifier
{

    [SerializeField] private string _uniqueID;
    public string UniqueID { get => _uniqueID; set => _uniqueID = value; }

    private void Start()
    {
        if(GameManager.Instance.IsCollectableCollected(UniqueID))
        {
            Destroy(gameObject);
        }
    }
    public void OnCollect(Player player)
    {
        player.GiveKey();
        GameManager.Instance.MarkCollectableColledted(UniqueID);
        Destroy(gameObject);
    }
}
