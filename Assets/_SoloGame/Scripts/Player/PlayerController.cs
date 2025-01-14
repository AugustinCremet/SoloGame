using BulletPro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] GameObject bullet;

    Player player;
    Camera cam;
    Rigidbody2D rb;
    Vector2 horizontalMovement;

    [SerializeField] GameObject _cursor;


    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = FindAnyObjectByType<Camera>();
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = horizontalMovement * moveSpeed;
        _cursor.transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>();
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            BulletEmitter bullet = GetComponent<BulletEmitter>();
            bullet.Play();
        }
    }
}
