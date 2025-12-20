using System.Xml;
using Unity.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HamiltonianSwitch : DoorBase
{
    [SerializeField] SceneField _puzzleScene;

    public override void Interact(Player player, PlayerController controller)
    {
        MySceneManager.Instance.LoadNewLevel(_puzzleScene, LevelType.Puzzle, () => { DoorManager.Instance.UnlockDoor(_uniqueID); });
    }
}
