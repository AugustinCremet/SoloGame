using UnityEngine;

public class ChestPop : MonoBehaviour, IPuzzleSolved
{
    [SerializeField] private GameObject _chest;
    public void OnPuzzleSolved()
    {
        _chest.SetActive(true);
    }
}
