using UnityEngine;

public abstract class DoorBase : MonoBehaviour, IInteractable, IUniqueIdentifier
{
    [SerializeField] protected string _uniqueID;
    public string UniqueID { get => _uniqueID; set => _uniqueID = value; }

    protected virtual void UnlockDoor(string id)
    {
        if (id == _uniqueID)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void Start()
    {
        DoorManager.OnDoorUnlocked += UnlockDoor;
        if (DoorManager.Instance.IsUnlocked(_uniqueID))
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDisable()
    {
        DoorManager.OnDoorUnlocked -= UnlockDoor;
    }

    public abstract void Interact(Player player, PlayerController controller);

    public void ShowPopup()
    {
        throw new System.NotImplementedException();
    }

    public void HidePopup()
    {
        throw new System.NotImplementedException();
    }
}
