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
    private float _gooCurrentCD = 0f;
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
    private Vector2 _cachedMovementVector;
    private bool _movementWasBlockedLastFrame = false;
    private Player _player;
    private Material _material;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] GameObject _crosshair;
    private Vector2 _screenSize;

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
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _material = _spriteRenderer.material;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        _screenSize = new Vector2(Screen.width, Screen.height);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            AdjustPlayerColor(1);
        }

        AdjustCrosshair();

        if (IsGoo || _dashCurrentCooldown != 0f)
        {
            StartDashTimers();
        }

        if (_movementWasBlockedLastFrame && !_player.SkillStateMachine.CurrentState.BlockMovement)
        {
            if (_cachedMovementVector.sqrMagnitude > 0.01f)
            {
                MovementVector = _cachedMovementVector;
                _player.SkillStateMachine.TryChangeState(_player.MovingState);
            }
            else
            {
                MovementVector = Vector2.zero;
                _player.SkillStateMachine.TryChangeState(_player.IdleState);
            }
        }
    }

    private void AdjustCrosshair()
    {
        Vector2 crosshairMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        _crosshair.transform.localPosition += new Vector3(crosshairMovement.x, crosshairMovement.y, 0f) * 0.5f;

        Vector3 crosshairScreenPos = _cam.WorldToScreenPoint(_crosshair.transform.localPosition);
        crosshairScreenPos.x = Mathf.Clamp(crosshairScreenPos.x, 0f, Screen.width);
        crosshairScreenPos.y = Mathf.Clamp(crosshairScreenPos.y, 0f, Screen.height);
        
        Debug.Log(crosshairScreenPos);

        Vector3 clampedWorldPos = _cam.ScreenToWorldPoint(crosshairScreenPos);
        clampedWorldPos.z = 0f;

        _crosshair.transform.localPosition = clampedWorldPos;
    }

    void FixedUpdate()
    {
        //_rb.linearVelocity = _horizontalMovement * _moveSpeed;
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        _cachedMovementVector = context.ReadValue<Vector2>();

        bool movementBlocked = _player.SkillStateMachine.CurrentState.BlockMovement;
        if (!movementBlocked)
        {
            MovementVector = context.ReadValue<Vector2>();
            if (MovementVector.sqrMagnitude > 0.01f)
            {
                //Make sure to stop the bullet
                _bullet.Stop();
                _player.SkillStateMachine.TryChangeState(_player.MovingState);
            }
            else
            {
                _player.SkillStateMachine.TryChangeState(_player.IdleState);
            }       
        }
        else
        {
            _movementWasBlockedLastFrame = true;
            MovementVector = Vector2.zero;
        }
    }

    public void StopMovement()
    {
        _rb.linearVelocity = Vector2.zero;
    }

    public void HandleMovement()
    {
        _rb.linearVelocity = MovementVector * _moveSpeed;
    }

    public void FireInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _player.SkillStateMachine.TryChangeState(_player.ShootingState);
            _isShooting = true;
        }
        else if (context.canceled)
        {
            _isShooting = false;
            if (MovementVector.magnitude <= 0f)
            {
                _player.SkillStateMachine.TryChangeState(_player.IdleState);
            }
            else
            {
                _player.SkillStateMachine.TryChangeState(_player.MovingState);
            }
        }
    }

    public void HandleShooting()
    {
        _bullet.Play();
    }
    public void StopShooting()
    {
        _bullet.Stop();
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

    public void CheckIfHitIsAvailable(BulletPro.Bullet bullet, Vector3 position)
    {
        int damageAmount = bullet.moduleParameters.GetInt("Damage");
        AdjustPlayerColor(damageAmount);
        _player.Damage(damageAmount);

        //TODO Need to create a IFrame
        bullet.Die();
    }

    public void AdjustPlayerColor(int amount)
    {
        const int COLOR_MAX_VARIATION = 510; // Green + Red values
        int healthVariation = _player.MaxHealth - 1;
        int currentHealthLost = _player.MaxHealth - _player.CurrentHealth;
        int colorVariation = COLOR_MAX_VARIATION / healthVariation;
        int variationAmount = amount * colorVariation + currentHealthLost * colorVariation;

        int redAmount = Mathf.Min(variationAmount, 255);
        int greenAmount = variationAmount <= 255 ? 255 : Mathf.Max(255 + (255 - variationAmount), 0);

        Debug.Log($"{redAmount}, {greenAmount}");

        _material.SetColor("_Color", new Color32((byte)redAmount, (byte)greenAmount, 0, 255));
        _player.Damage(1);
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
