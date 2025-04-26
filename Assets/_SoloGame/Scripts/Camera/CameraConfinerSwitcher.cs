using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraConfinerSwitcher : MonoBehaviour
{
    private CinemachineConfiner2D _confiner;

    void Awake()
    {
        _confiner = GetComponent<CinemachineConfiner2D>();
    }

    public void ChangeBoundary(Collider2D newBoundary)
    {
        _confiner.m_BoundingShape2D = newBoundary;
        _confiner.InvalidateCache();
    }
}
