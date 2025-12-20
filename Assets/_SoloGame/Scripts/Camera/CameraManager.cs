using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] private CinemachineConfiner2D _confiner;
    [SerializeField] private CinemachineGroupFraming _groupFraming;

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
}
