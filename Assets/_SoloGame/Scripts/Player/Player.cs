using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;
using BulletPro;
using System.Collections;
using Unity.VisualScripting;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum PlayerAbilities
{
    None           = 0,
    Dash           = 1 << 0,
    ColorSwitch    = 1 << 1,
    BulletTime     = 1 << 2,
    BouncingBullet = 1 << 3,
}
public class Player : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] int _maxHealth = 100;
    [SerializeField] float _maxGoo = 10f;
    [SerializeField] float _gooRecoveryRate = 0.25f;
    [Space(10)]
    [Header("Ressource cost")]
    [SerializeField] float _gooPerSecForGooState = 1f;
    [Space(10)]
    [Header("Cooldowns")]
    [SerializeField] float _invinsibilityDuration = 10f;
    [SerializeField] float _shootingCDDuration = 1f;

    public int MaxHealth => _maxHealth;
    private int _currentHealth = 0;
    public int CurrentHealth => _currentHealth;
    private bool _isInvinsible = false;
    public float MaxGoo => _maxGoo;
    private float _currentGoo;
    private bool _isUsingGoo;

    private Crosshair _crosshair;
    private bool _willShootAgain;
    private bool _isShooting;

    //CD
    private Cooldown _shootingCD;

    [Space(10)]
    [Header("Bullet Emitter")]
    [SerializeField] EmitterProfile _normalProfile;
    [SerializeField] float _normalProfileAnimationSpeed = 1.5f;
    [SerializeField] EmitterProfile _emergencyShot;
    [SerializeField] float _emergencyShotAnimationSpeed = 1f;
    [SerializeField] EmitterProfile _bouncingProfile;

    [Space(10)]
    [Header("Others")]
    [SerializeField] GameObject _crosshairGO;

    // Components
    private BulletEmitter _bulletEmitter;
    private PlayerController _playerController;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;
    private Material _material;
    public PlayerAbilities Abilities { get; private set; }

    // FSM
    public StateMachine StateMachine;
    public IdleSkillState IdleState;
    public GooState GooState;
    public ShootingState ShootingState;
    public MovingState MovingState;
    public DeadState DeadState;
    public HitState HitState;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerController = GetComponent<PlayerController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _crosshair = _crosshairGO.GetComponent<Crosshair>();
        _material = _spriteRenderer.material;
        _currentHealth = _maxHealth;
        _currentGoo = _maxGoo;

        // CD
        _shootingCD = new Cooldown(_shootingCDDuration);

        //State machine with states
        StateMachine = new StateMachine();
        IdleState = new IdleSkillState(_playerController, this, _animator);
        GooState = new GooState(_playerController, this, _animator);
        ShootingState = new ShootingState(_playerController, this, _animator);
        MovingState = new MovingState(_playerController, this, _animator);
        DeadState = new DeadState(_playerController, this, _animator);
        HitState = new HitState(_playerController, this, _animator);
    }
    private void Start()
    {
        UIManager.Instance.ChangeMaxHealth(_maxHealth);
        UIManager.Instance.ChangeCurrentHealth(_currentHealth);
        UIManager.Instance.ChangeMaxGoo(_maxGoo);
        UIManager.Instance.ChangeCurrentGoo(_currentGoo);


        _bulletEmitter = GetComponent<BulletEmitter>();
        // Make sure the emitter will be active **Weird fix but okay**
        _bulletEmitter.Play();
        _bulletEmitter.Stop();
        StateMachine?.SetInitialState(IdleState);


        // AC_TODO remove
        //_bulletEmitter.SwitchProfile(_normalProfile);
        //GrantAbility(PlayerAbilities.BouncingBullet);
    }

    private void Update()
    {
        StateMachine?.Update();
        RecoverGooOverTime(_gooRecoveryRate);

        if (_shootingCD.IsReady && _willShootAgain)
        {
            StateMachine.TryChangeState(ShootingState);
        }
    }

    private void FixedUpdate()
    {
        StateMachine?.FixedUpdate();
    }

    private void OnEnable()
    {
        GameManager.OnSave += SavePlayerData;
        GameManager.OnLoad += LoadPlayerData;
        ResetPlayer();
    }

    private void OnDisable()
    {
        GameManager.OnSave -= SavePlayerData;
        GameManager.OnLoad -= LoadPlayerData;
    }

    public void GrantAbility(PlayerAbilities ability)
    {
        Abilities |= ability;

        if(ability == PlayerAbilities.BouncingBullet)
        {
            _bulletEmitter.SwitchProfile(_bouncingProfile);
        }
    }

    public void RemoveAbility(PlayerAbilities ability)
    {
        Abilities &= ~ability;
    }

    public bool HasAbility(PlayerAbilities ability)
    {
        return (Abilities & ability) == ability;
    }
    public void ReevaluateState()
    {
        if (_playerController.MovementVector.magnitude <= 0f)
        {
            StateMachine.TryChangeState(IdleState);
        }
        else
        {
            StateMachine.TryChangeState(MovingState);
        }

        if (_isShooting)
        {
            StateMachine.TryChangeState(ShootingState);
        }
    }
    public void StartMovement()
    {
        //Make sure to stop the bullet
        _bulletEmitter.Stop();
        StateMachine.TryChangeState(MovingState);
    }
    public void HandleMovement()
    {
        _rb.linearVelocity = _playerController.MovementVector * _moveSpeed;
    }
    public void StopMovement()
    {
        _rb.linearVelocity = Vector2.zero;
    }
    public Vector2 OnMovementUnblocked(Vector2 cachedVector)
    {
        if (cachedVector.sqrMagnitude > 0.01f)
        {
            StateMachine.TryChangeState(MovingState);
            return cachedVector;
        }
        else
        {
            StateMachine.TryChangeState(IdleState);
            return Vector2.zero;
        }
    }
    public void StartGoo()
    {
        if (_currentGoo < _gooPerSecForGooState)
            return;

        StateMachine.TryChangeState(GooState);
    }
    public void HandleGoo()
    {
        _isUsingGoo = true;
        if (!LoseGooOverTime(_gooPerSecForGooState))
        {
            _isUsingGoo = false;
            ReevaluateState();
        }
    }
    public void StopGoo()
    {
        _isUsingGoo = false;
        ReevaluateState();
    }
    public void StartShooting()
    {
        _willShootAgain = true;
        if(_shootingCD.IsReady)
        {
            StateMachine.TryChangeState(ShootingState);
            _isShooting = true;
        }
    }
    private int _lastShootFrame = -1;
    public void HandleShootingEvent()
    {
        int currentFrame = Time.frameCount;
        if (_lastShootFrame == currentFrame)
        {
            Debug.LogWarning("HandleShooting event has tried to be called twice in one frame");
            return;
        }
        _lastShootFrame = currentFrame;

        _crosshair.StartCooldown(_shootingCDDuration);
        _shootingCD.Use();
        _bulletEmitter.Play();
        if (CurrentHealth > 1)
        {
            LoseSlimeBall(1);
        }
    }
    public void StopShooting()
    {
        _willShootAgain = false;
    }
    public void StopShootingEvent()
    {
        Debug.Log("!Shooting");
        _isShooting = false;
        _bulletEmitter.Stop();
        if (!_willShootAgain || !_shootingCD.IsReady)
        {
            ReevaluateState();
        }
    }
    public void Damage(int dmgAmount)
    {
        if (StateMachine.CurrentState == HitState || _isInvinsible)
            return;

        LoseSlimeBall(dmgAmount);
        if (_currentHealth <= 0)
        {
            StateMachine.TryChangeState(DeadState);
        }

        StateMachine.TryChangeState(HitState);
    }

    public void LoseSlimeBall(int amount)
    {
        _currentHealth -= amount;
        if (_currentHealth == 1)
        {
            StartCoroutine(SwitchProfileNextFrame(_emergencyShot));
            _animator.SetFloat("ShootingSpeed", _emergencyShotAnimationSpeed);
        }
        UIManager.Instance.ChangeCurrentHealth(_currentHealth);
    }

    // Wait one frame before switching profile so if a bullet was fire during the same frame, it is not affected by the switch
    private IEnumerator SwitchProfileNextFrame(EmitterProfile newProfile)
    {
        yield return null;
        _bulletEmitter.SwitchProfile(newProfile);
    }

    public void Heal(int healAmount)
    {
        if(_currentHealth < _maxHealth)
        {
            _currentHealth += healAmount;
            UIManager.Instance.ChangeCurrentHealth(_currentHealth);
        }

        if(_currentHealth > 1 && _bulletEmitter.emitterProfile != _normalProfile)
        {
            Debug.Log("Back to normal profile");
            StartCoroutine(SwitchProfileNextFrame(_normalProfile));
            _animator.SetFloat("ShootingSpeed", _normalProfileAnimationSpeed);
        }
    }

    public bool LoseGooOverTime(float amountPerSec)
    {
        _currentGoo -= amountPerSec * Time.deltaTime;
        UIManager.Instance.ChangeCurrentGoo(_currentGoo);

        if (_currentGoo <= 0f)
        {
            _currentGoo = 0f;
            return false;
        }

        return true;
    }

    public void LoseGoo(float amount)
    {

    }

    public void RecoverGooOverTime(float amountPerSec)
    {
        if (_currentGoo >= _maxGoo || _isUsingGoo)
            return;

        Debug.Log("Recovering");
        _currentGoo += amountPerSec * Time.deltaTime;
        UIManager.Instance.ChangeCurrentGoo((_currentGoo));

        if(_currentGoo >= _maxGoo)
        {
            _currentGoo = _maxGoo;
        }
    }

    public void StartInvincibility()
    {
        _isInvinsible = true;
        StartCoroutine(InvinsibilityCR());
    }

    private IEnumerator InvinsibilityCR()
    {
        float timer = 0f;
        while(timer < _invinsibilityDuration)
        {
            _spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            _spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
            timer += 0.2f;
        }
        _isInvinsible = false;
    }

    public void HandleDeath()
    {
        StartCoroutine(HandleDeathCR());
    }

    public IEnumerator HandleDeathCR()
    {
        yield return new WaitForSecondsRealtime(2f);
        SceneTransition sceneTransition = FindAnyObjectByType<SceneTransition>();
        yield return StartCoroutine(sceneTransition.FadeIn());
        HandleDeathAfterFadeIn();
    }

    public void HandleDeathAfterFadeIn()
    {
        GameManager.Instance.LoadGame();
    }

    public void ResetPlayer()
    {
        _currentHealth = _maxHealth;
        //UIManager.Instance.ChangeCurrentHealth(_currentHealth);
        _animator.Rebind();
        ResetPlayerColor();
        StateMachine.ResetStates(IdleState);
    }

    public void SetPosition(Transform newTransform)
    {
        gameObject.transform.position = newTransform.position;
    }

    public void CheckIfHitIsAvailable(BulletPro.Bullet bullet, Vector3 position)
    {
        int damageAmount = bullet.moduleParameters.GetInt("Damage");
        AdjustPlayerColor(damageAmount);
        Damage(damageAmount);

        bullet.Die();
    }

    public void AdjustPlayerColor(int amount)
    {
        const int COLOR_MAX_VARIATION = 510; // Green + Red values
        int healthVariation = MaxHealth - 1;
        int currentHealthLost = MaxHealth - CurrentHealth;
        int colorVariation = COLOR_MAX_VARIATION / healthVariation;
        int variationAmount = amount * colorVariation + currentHealthLost * colorVariation;

        int redAmount = Mathf.Min(variationAmount, 255);
        int greenAmount = variationAmount <= 255 ? 255 : Mathf.Max(255 + (255 - variationAmount), 0);

        _material.SetColor("_Color", new Color32((byte)redAmount, (byte)greenAmount, 0, 255));
        Damage(1);
    }

    public void ResetPlayerColor()
    {
        _material.SetColor("_Color", new Color32(0, 255, 0, 255));
    }

    public SaveData SavePlayerData()
    {
        SaveData data = new SaveData
        {
            PlayerData = new PlayerData
            {
                hp = _maxHealth,
            }
        };
        return data;
    }

    public void LoadPlayerData(SaveData saveData)
    {
        ResetPlayer();
        //_currentHealth = saveData.PlayerData.hp;
        //UIManager.Instance.ChangeCurrentHealth(_currentHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ICollectable collectable = collision.gameObject.GetComponent<ICollectable>();
        if (collectable != null)
        {
            collectable.OnCollect(gameObject);
        }
    }
}
