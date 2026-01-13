using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] private CinemachineConfiner2D _confiner;
    [SerializeField] private CinemachineGroupFraming _groupFraming;
    [SerializeField] private CinemachineCamera _camera;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void SetBoundary(CompositeCollider2D newBoundary)
    {
        _confiner.BoundingShape2D = newBoundary;
    }

    public void SetOrthoSize(float newSize)
    {
        _groupFraming.OrthoSizeRange = new Vector2(newSize, newSize);
    }

    public void SetFixedRoomBoundary(BoxCollider2D roomCollider)
    {
        Bounds bounds = roomCollider.bounds;

        Vector3 center = bounds.center;
        _camera.ForceCameraPosition(center, Quaternion.identity);

        float roomHeight = bounds.size.y;
        _camera.Lens.OrthographicSize = roomHeight / 2f;

        _confiner.BoundingShape2D = roomCollider;
        _confiner.InvalidateBoundingShapeCache();
    }
}
