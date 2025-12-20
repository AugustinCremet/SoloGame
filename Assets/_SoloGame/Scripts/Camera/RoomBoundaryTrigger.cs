using UnityEngine;

public class RoomBoundaryTrigger : MonoBehaviour
{
    [SerializeField] private CompositeCollider2D _confiner;
    [SerializeField] private float _camSize = 8.4375f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            CameraManager.Instance.SetBoundary(_confiner);
            CameraManager.Instance.SetOrthoSize(_camSize);
            FloorStateManager.Instance.ResetUncompletedPuzzles();
        }
    }
}
