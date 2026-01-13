using UnityEngine;

public class Staircase : MonoBehaviour
{
    [SerializeField] private int _targetFloorIndex;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindAnyObjectByType<FloorStateManager>().ChangeFloor(_targetFloorIndex);
    }
}
