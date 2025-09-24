using UnityEngine;

public class LockedDoor : MonoBehaviour, IInteractable, IUniqueIdentifier
{

    [SerializeField] string _uniqueID;
    public string UniqueID { get => _uniqueID; set => _uniqueID = value; }


    public void Interact(Player player)
    {
        if(player.HasKey)
        {
            DoorManager.Instance.UnlockDoor(_uniqueID);
            player.UseKey();
        }
        else
        {
            player.gameObject.GetComponentInChildren<ChatBubble>().SetText("I got no key");
        }
    }

    private void UnlockDoor(string id)
    {
        if(id == _uniqueID)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        DoorManager.OnDoorUnlocked += UnlockDoor;
        if(DoorManager.Instance.IsUnlocked(_uniqueID))
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        DoorManager.OnDoorUnlocked -= UnlockDoor;
    }
}
