using BulletPro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public static Action ChangeColor;
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _dashSpeed = 20f;
    [SerializeField] float _dashDuration = 0.2f;
    float _dashCurrentDuration;
    [SerializeField] float _dashCooldown = 1f;
    float _dashCurrentCooldown;
    public bool isDashing { get; private set; }
    Vector2 _dashDirection;

    Camera _cam;
    Rigidbody2D _rb;
    Vector2 _horizontalMovement;

    [SerializeField] GameObject _cursor;
    private BulletEmitter _bullet = null;
    private bool _isShooting;
    private float _lastShotTime = 0f;

    public static event Action OnInteract;


    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _cam = FindAnyObjectByType<Camera>();
        _bullet = GetComponent<BulletEmitter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_cam != null)
        {
            _cursor.transform.position = _cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if(isDashing || _dashCurrentCooldown != 0f)
        {
            StartDashTimers();
        }
        if(_isShooting/* && Time.realtimeSinceStartup - _lastShotTime > 1*/)
        {
            _bullet.Play();
            _lastShotTime = Time.realtimeSinceStartup;
        }
        else
        {
            //_bullet.Stop();
        }
    }

    void FixedUpdate()
    {
        _rb.linearVelocity = _horizontalMovement * _moveSpeed;

        if(isDashing)
        {
            DashMovement();
        }
    }

    public void SwitchColor(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            ChangeColor?.Invoke();
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
            _isShooting = true;
        }
        else if (context.canceled)
        {
            _isShooting = false;
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (!isDashing && context.performed && _dashCurrentCooldown == 0f)
        {
            isDashing = true;
            _dashDirection = _horizontalMovement;
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            OnInteract?.Invoke();
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
            _rb.linearVelocity = Vector2.zero;
        }
    }

    void DashMovement()
    {
        if(_dashCurrentDuration <= _dashDuration)
        {
            _rb.AddForce(_dashDirection * _dashSpeed, ForceMode2D.Impulse);
        }
        else
        {
            isDashing = false;
        }
    }
}
