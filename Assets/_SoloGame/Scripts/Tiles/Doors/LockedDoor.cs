using UnityEngine;

public class LockedDoor : DoorBase
{
    public override void Interact(Player player, PlayerController controller)
    {
        if (player.HasKey)
        {
            DoorManager.Instance.UnlockDoor(_uniqueID);
            player.UseKey();
        }
        else
        {
            player.StartChat(new string[] { "I have no key" });
        }
    }
}
