using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraConfinerSwitcher : MonoBehaviour
{
    private CinemachineConfiner2D _confiner;
    private CinemachineCamera _vcam;

    void Awake()
    {
        _confiner = GetComponent<CinemachineConfiner2D>();
        _vcam = GetComponent<CinemachineCamera>();
    }

    public void ChangeBoundary(PolygonCollider2D newBoundary, float cameraSize = 8.4375f)
    {
        // Precaution
        _confiner.BoundingShape2D = null;
        _confiner.InvalidateBoundingShapeCache();

        // Now assign the real collider
        _confiner.BoundingShape2D = newBoundary;
        _confiner.InvalidateBoundingShapeCache();

        GetComponent<CinemachineGroupFraming>().OrthoSizeRange = new Vector2(cameraSize, cameraSize);
        //_vcam.Lens.OrthographicSize = cameraSize;
    }

    public void AutomaticallyAssignCameraSize(Tilemap tilemap, float padding = 0.5f)
    {
        // Get collider from tilemap
        var collider = tilemap.GetComponent<Collider2D>();
        if (collider == null)
        {
            Debug.LogWarning("Tilemap has no Collider2D for confiner!");
            return;
        }

        // Assign collider to Cinemachine confiner
        _confiner.BoundingShape2D = collider;
        _confiner.InvalidateBoundingShapeCache();

        // Get bounds for camera size calculation
        Bounds roomBounds = collider.bounds;
        Debug.Log(roomBounds.size.x + " " + roomBounds.size.y);

        float roomWidth = roomBounds.size.x + padding * 2f;
        float roomHeight = roomBounds.size.y + padding * 2f;

        // Orthographic size
        float orthoSize = roomHeight / 2f;

        // Fit width as well
        float screenAspect = (float)Screen.width / Screen.height;
        float orthoSizeForWidth = roomWidth / (2f * screenAspect);

        // Take the larger size
        _vcam.Lens.OrthographicSize = Mathf.Max(orthoSize, orthoSizeForWidth);
    }
}
