using BulletPro;
using Newtonsoft.Json.Serialization;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static Action ChangeColor;

    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _gooPerSecForGooState = 1f;
    public bool IsUsingGoo {  get; private set; }

    Camera _cam;
    Rigidbody2D _rb;
    public Vector2 MovementVector { get; private set; }
    private Vector2 _cachedMovementVector;
    private bool _movementWasBlockedLastFrame = false;
    private Player _player;
    private Material _material;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] GameObject _crosshair;
    private const float CROSSHAIR_NORMAL_SPEED = 0.5f;
    private const float CROSSHAIR_AIMING_SPEED = 0.05f;

    private BulletEmitter _bullet = null;
    private bool _isShooting;

    public static event Action OnInteract;
    private Animator _animator;


    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _cam = FindAnyObjectByType<Camera>();
        _bullet = GetComponent<BulletEmitter>();
        _player = GetComponent<Player>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();    
        _material = _spriteRenderer.material;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            AdjustPlayerColor(1);
        }

        AdjustCrosshair();

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
            _movementWasBlockedLastFrame = false;   
        }
    }

    private void AdjustCrosshair()
    {
        Vector2 crosshairMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        _crosshair.transform.position += new Vector3(crosshairMovement.x, crosshairMovement.y, 0f) * CROSSHAIR_NORMAL_SPEED;

        Vector3 crosshairScreenPos = _cam.WorldToScreenPoint(_crosshair.transform.position);
        crosshairScreenPos.x = Mathf.Clamp(crosshairScreenPos.x, 0f, Screen.width);
        crosshairScreenPos.y = Mathf.Clamp(crosshairScreenPos.y, 0f, Screen.height);

        Vector3 clampedWorldPos = _cam.ScreenToWorldPoint(crosshairScreenPos);
        clampedWorldPos.z = 0f;

        _crosshair.transform.position = clampedWorldPos;
    }

    void FixedUpdate()
    {
        //_rb.linearVelocity = _horizontalMovement * _moveSpeed;
    }

    public void ReevaluateState()
    {
        if (MovementVector.magnitude <= 0f)
        {
            _player.SkillStateMachine.TryChangeState(_player.IdleState);
        }
        else
        {
            _player.SkillStateMachine.TryChangeState(_player.MovingState);
        }

        if(_isShooting)
        {
            _player.SkillStateMachine.TryChangeState(_player.ShootingState);
        }
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        _cachedMovementVector = context.ReadValue<Vector2>();

        bool movementBlocked = _player.SkillStateMachine.CurrentState.BlockMovement;
        
        if (!movementBlocked)
        {
            MovementVector = context.ReadValue<Vector2>();
            if (_player.SkillStateMachine.CurrentState == _player.GooState)
                return;

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
            StopShooting();
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

    private int _lastShootFrame = -1;
    public void HandleShooting()
    {
        int currentFrame = Time.frameCount;
        if (_lastShootFrame == currentFrame)
        {
            Debug.LogWarning("HandleShooting event has tried to be called twice in one frame");
            return;
        }
        _lastShootFrame = currentFrame;

        AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);
        if (_player.CurrentHealth > 1)
        {
            _bullet.Play();
            _player.LoseSlimeBall(1);
        }
    }

    public void StopShooting()
    {
        _bullet.Stop();
    }

    public void DashInput(InputAction.CallbackContext context)
    {
        if (_player.CurrentGoo < _gooPerSecForGooState)
            return;

        if (context.performed)
        {
            _player.SkillStateMachine.TryChangeState(_player.GooState);
        }
        else if(context.canceled)
        {
            IsUsingGoo = false;
            ReevaluateState();
        }
    }
    public void HandleGoo()
    {
        IsUsingGoo = true;
        if(!_player.LoseGooOverTime(_gooPerSecForGooState))
        {
            IsUsingGoo = false;
            ReevaluateState();
        }
    }

    public void InteractInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            OnInteract?.Invoke();
        }
    }

    public void CheckIfHitIsAvailable(BulletPro.Bullet bullet, Vector3 position)
    {
        int damageAmount = bullet.moduleParameters.GetInt("Damage");
        AdjustPlayerColor(damageAmount);
        _player.Damage(damageAmount);

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

        _material.SetColor("_Color", new Color32((byte)redAmount, (byte)greenAmount, 0, 255));
        _player.Damage(1);
    }

    public void ResetPlayerColor()
    {
        _material.SetColor("_Color", new Color32(0, 255, 0, 255));
    }
}
