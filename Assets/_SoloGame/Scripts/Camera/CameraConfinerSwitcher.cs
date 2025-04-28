using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Cinemachine;
using UnityEngine;

public class CameraConfinerSwitcher : MonoBehaviour
{
    private CinemachineConfiner2D _confiner;
    private float _transitionDuration = 2f;

    void Awake()
    {
        _confiner = GetComponent<CinemachineConfiner2D>();
    }

    public void ChangeBoundary(PolygonCollider2D newBoundary)
    {
        _confiner.BoundingShape2D = newBoundary;
        _confiner.InvalidateBoundingShapeCache();
    }

    public void StartBoundaryChangeAnimation(PolygonCollider2D previousBoundary, PolygonCollider2D newBoundary)
    {
        StartCoroutine(SmoothBoundaryTransition(previousBoundary, newBoundary));
    }

    IEnumerator SmoothBoundaryTransition(PolygonCollider2D previousBoundary, PolygonCollider2D newBoundary)
    {

        Debug.Log("Start coroutine");
        float elapsedTime = 0f;

        PolygonCollider2D initialBoundary = previousBoundary;


        // Smoothly transition to the new boundary
        while (elapsedTime < _transitionDuration)
        {
            Debug.Log("Loop");
            float lerpFactor = elapsedTime / _transitionDuration;
            _confiner.BoundingShape2D = LerpBoundary(initialBoundary, newBoundary, lerpFactor);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final boundary is set
        _confiner.BoundingShape2D = newBoundary;

        Debug.Log("End coroutine");
    }

    PolygonCollider2D LerpBoundary(PolygonCollider2D from, PolygonCollider2D to, float t)
    {
        // Create a new PolygonCollider2D to hold the lerped vertices
        PolygonCollider2D lerpedCollider = gameObject.AddComponent<PolygonCollider2D>();

        // Get the number of vertices for the current boundary (assuming both have the same number of vertices)
        int vertexCount = from.points.Length;
        Vector2[] lerpedVertices = new Vector2[vertexCount];

        for (int i = 0; i < vertexCount; i++)
        {
            // Lerp between the corresponding vertices of the two colliders
            lerpedVertices[i] = Vector2.Lerp(from.points[i], to.points[i], t);
        }

        // Set the lerped vertices on the new collider
        lerpedCollider.SetPath(0, lerpedVertices);

        // Return the new collider with the lerped boundary
        return lerpedCollider;
    }
}
