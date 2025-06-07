using BulletPro;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static Action ChangeColor;
    [Header("Goo Settings")]
    [SerializeField] GameObject _gooPrefab;
    [SerializeField] float _gooCD;
    float _gooCurrentCD = 0f;
    [Space(10)]

    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _dashSpeed = 20f;
    [SerializeField] float _dashDuration = 0.2f;
    float _dashCurrentDuration;
    [SerializeField] float _dashCooldown = 1f;
    float _dashCurrentCooldown;
    public bool IsGoo { get; private set; }
    Vector2 _dashDirection;

    Camera _cam;
    Rigidbody2D _rb;
    public Vector2 MovementVector { get; private set; }
    Player _player;

    [SerializeField] GameObject _cursor;
    private BulletEmitter _bullet = null;
    private bool _isShooting;

    public static event Action OnInteract;


    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _cam = FindAnyObjectByType<Camera>();
        _bullet = GetComponent<BulletEmitter>();
        _player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_cam != null)
        {
            _cursor.transform.position = _cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if(IsGoo || _dashCurrentCooldown != 0f)
        {
            StartDashTimers();
        }
    }

    void FixedUpdate()
    {
        //_rb.linearVelocity = _horizontalMovement * _moveSpeed;
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        MovementVector = context.ReadValue<Vector2>();
        if (MovementVector.x < 0f ||
           MovementVector.x > 0f ||
           MovementVector.y < 0f ||
           MovementVector.y > 0f)
        {
            _player.MovementStateMachine.TryChangeState(_player.MovingState);
        }
        else
        {
            _player.MovementStateMachine.TryChangeState(_player.IdleMovementState);
        }
    }

    public void StopMovement()
    {
        _rb.linearVelocity = Vector2.zero;
    }

    public void HandleMovement()
    {
        _rb.linearVelocity = MovementVector * _moveSpeed;
        //if(_horizontalMovement.x > 0f)
        //{
        //    transform.localScale = new Vector3(1f, 1f, 1f);
        //}    
        //else if(_horizontalMovement.x < 0f)
        //{
        //    transform.localScale = new Vector3(-1f, 1f, 1f);
        //}
    }

    public void FireInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(GameObject.FindWithTag("AimSight").transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            _player.SkillStateMachine.TryChangeState(_player.ShootingState);
        }
        else if (context.canceled)
        {
            _player.SkillStateMachine.TryChangeState(_player.IdleSkillState);
        }
    }

    public void HandleShooting()
    {
        _bullet.Play();
    }

    public void DashInput(InputAction.CallbackContext context)
    {
        if (context.performed && _dashCurrentCooldown == 0f)
        {
            _player.SkillStateMachine.TryChangeState(_player.GooState);
            //IsGoo = true;
        }
    }

    public void HandleGoo()
    {
        //GetComponentInChildren<ParticleSystem>().Play();
        if(_gooCurrentCD <= 0)
        {
            GameObject instance = Instantiate(_gooPrefab, transform.position, Quaternion.identity);
            _gooCurrentCD = _gooCD;
            Destroy(instance, 3f);
        }
        else
        {
            _gooCurrentCD -= Time.deltaTime;
        }
    }

    public void InteractInput(InputAction.CallbackContext context)
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
            IsGoo = false;
        }
    }
}
