using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSight : MonoBehaviour
{
    Transform _playerTransform;
    SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (_playerTransform.position - transform.position).normalized;
        transform.up = direction; // Rotates the laser to face the player

        float laserLength = Vector2.Distance(_playerTransform.position, transform.position);
        _spriteRenderer.size = new Vector2(_spriteRenderer.size.x, laserLength);
    }
}
