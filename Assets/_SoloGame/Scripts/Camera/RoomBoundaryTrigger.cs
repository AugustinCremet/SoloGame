using UnityEngine;

public class RoomBoundaryTrigger : MonoBehaviour
{
    [SerializeField] private CompositeCollider2D _confiner;
    [SerializeField] private float _camSize = 7.5f;
    [SerializeField] private bool _isFixedCam = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(!_isFixedCam)
            {
                CameraManager.Instance.SetBoundary(_confiner);
            }
            else
            {
                CameraManager.Instance.SetFixedRoomBoundary(GetComponent<BoxCollider2D>());
            }

            CameraManager.Instance.SetOrthoSize(_camSize);
            //FloorStateManager.Instance.ResetUncompletedPuzzles();
        }
    }
}
