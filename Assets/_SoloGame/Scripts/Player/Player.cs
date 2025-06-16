using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;
using BulletPro;
using System.Collections;
using Unity.VisualScripting;

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
    [SerializeField] int _maxHealth = 100;
    public int MaxHealth { get { return _maxHealth; } }
    private int _currentHealth = 0;
    public int CurrentHealth { get { return _currentHealth; } }
    private BulletEmitter _bulletEmitter;
    private PlayerController _playerController;
    private Animator _animator;
    [SerializeField] EmitterProfile _normalProfile;
    [SerializeField] EmitterProfile _bouncingProfile;
    public PlayerAbilities Abilities { get; private set; }
    private IDataService _dataService = new JsonDataService();

    public SkillStateMachine SkillStateMachine;
    public IdleSkillState IdleState;
    public GooState GooState;
    public ShootingState ShootingState;
    public MovingState MovingState;
    public DeadState DeadState;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerController = GetComponent<PlayerController>();
        _currentHealth = _maxHealth;

        //State machine with states
        SkillStateMachine = new SkillStateMachine();
        IdleState = new IdleSkillState(_playerController, this, _animator);
        GooState = new GooState(_playerController, this, _animator);
        ShootingState = new ShootingState(_playerController, this, _animator);
        MovingState = new MovingState(_playerController, this, _animator);
        DeadState = new DeadState(_playerController, this, _animator);
    }
    private void Start()
    {

        UIManager.Instance.ChangeCurrentHealth(_currentHealth);
        UIManager.Instance.ChangeMaxHealth(_maxHealth);

        // TODO remove
        _bulletEmitter = GetComponent<BulletEmitter>();
        //_bulletEmitter.SwitchProfile(_normalProfile);
        //GrantAbility(PlayerAbilities.BouncingBullet);

        SkillStateMachine?.SetInitialState(IdleState);
    }

    private void Update()
    {
        SkillStateMachine?.Update();
    }

    private void FixedUpdate()
    {
        SkillStateMachine?.FixedUpdate();
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
    public void Damage(int dmgAmount)
    {
        _currentHealth = _currentHealth - dmgAmount;
        UIManager.Instance.ChangeCurrentHealth(_currentHealth);

        if (_currentHealth <= 0)
        {
            SkillStateMachine.TryChangeState(DeadState);
        }
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
        _playerController.ResetPlayerColor();
        SkillStateMachine.ResetStates(IdleState);
    }

    public void SetPosition(Transform newTransform)
    {
        gameObject.transform.position = newTransform.position;
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
}
