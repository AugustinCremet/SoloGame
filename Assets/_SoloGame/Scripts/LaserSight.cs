using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class LaserSight : MonoBehaviour
{
    Transform _playerTransform;
    LineRenderer _lineRenderer;
    private bool _doOnce;
    private int _layerMask;

    // Start is called before the first frame update
    void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
        _lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
{
        if (UtilityFunctions.IsPlayerInSight(transform.position, _playerTransform))
        {
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, _playerTransform.position);
        }
        else
        {
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, transform.position);
        }
    }
}
