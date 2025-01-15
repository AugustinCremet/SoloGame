using BulletPro;
using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _dashSpeed = 20f;
    [SerializeField] float _dashDuration = 0.2f;
    float _dashCurrentDuration;
    [SerializeField] float _dashCooldown = 1f;
    float _dashCurrentCooldown;
    bool _isDashing;
    Vector2 _dashDirection;
    [SerializeField] GameObject _bullet;

    Camera _cam;
    Rigidbody2D _rb;
    Vector2 _horizontalMovement;

    [SerializeField] GameObject _cursor;


    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _cam = FindAnyObjectByType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        _cursor.transform.position = _cam.ScreenToWorldPoint(Input.mousePosition);

        if(_isDashing)
        {
            StartDashTimers();
        }
    }

    void FixedUpdate()
    {
        _rb.velocity = _horizontalMovement * _moveSpeed;

        if(_isDashing)
        {
            DashMovement();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        _horizontalMovement = context.ReadValue<Vector2>();
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            BulletEmitter bullet = GetComponent<BulletEmitter>();
            bullet.Play();
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (!_isDashing && context.performed)
        {
            _isDashing = true;
            _dashDirection = _horizontalMovement;
        }
    }

    void StartDashTimers()
    {
        _dashCurrentCooldown += Time.deltaTime;
        _dashCurrentDuration += Time.deltaTime;

        if(_dashCurrentCooldown >= _dashCooldown)
        {
            _dashCurrentCooldown = 0;
            _dashCurrentDuration = 0;
            _isDashing = false;
            _rb.velocity = Vector2.zero;
        }
    }

    void DashMovement()
    {
        if(_dashCurrentDuration <= _dashDuration)
        {
            _rb.AddForce(_dashDirection * _dashSpeed, ForceMode2D.Impulse);
        }
    }
}
