using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapPoint : MonoBehaviour
{
    private Collider2D collider;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        Debug.Log(collider);
    }

    private void Update()
    {
    }
}
