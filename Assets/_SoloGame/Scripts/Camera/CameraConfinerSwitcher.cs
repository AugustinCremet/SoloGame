using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraConfinerSwitcher : MonoBehaviour
{
    private CinemachineConfiner2D _confiner;
    private CinemachineCamera _vcam;

    void Awake()
    {
        _confiner = GetComponent<CinemachineConfiner2D>();
        _vcam = GetComponent<CinemachineCamera>();
    }

    public void ChangeBoundary(PolygonCollider2D newBoundary, float cameraSize)
    {
        // Precaution
        _confiner.BoundingShape2D = null;
        _confiner.InvalidateBoundingShapeCache();

        // Now assign the real collider
        _confiner.BoundingShape2D = newBoundary;
        _confiner.InvalidateBoundingShapeCache();

        _vcam.Lens.OrthographicSize = cameraSize;
    }
}
