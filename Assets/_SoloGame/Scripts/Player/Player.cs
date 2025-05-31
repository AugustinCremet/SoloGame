using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;
using BulletPro;

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
    private int _currentHealth = 0;
    private BulletEmitter _bulletEmitter;
    private PlayerController _playerController;
    private Animator _animator;
    [SerializeField] EmitterProfile _normalProfile;
    [SerializeField] EmitterProfile _bouncingProfile;
    public PlayerAbilities Abilities { get; private set; }
    private IDataService _dataService = new JsonDataService();

    public SkillStateMachine SkillStateMachine;

    public MovementStateMachine MovementStateMachine;
    public IdleMovementState IdleMovementState;
    public MovingState MovingState;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerController = GetComponent<PlayerController>();
        _currentHealth = _maxHealth;

        //State machines
        SkillStateMachine = new SkillStateMachine(_playerController, _animator);
        MovementStateMachine = new MovementStateMachine(_playerController, _animator);
        IdleMovementState = new IdleMovementState(_playerController, _animator);
        MovingState = new MovingState(_playerController, _animator);
    }
    private void Start()
    {

        UIManager.Instance.ChangeCurrentHealth(_currentHealth);
        UIManager.Instance.ChangeMaxHealth(_maxHealth);

        // TODO remove
        _bulletEmitter = GetComponent<BulletEmitter>();
        _bulletEmitter.SwitchProfile(_normalProfile);
        //GrantAbility(PlayerAbilities.BouncingBullet);

        //_movementStateMachine?.Start();
        SkillStateMachine?.Start();
    }

    private void Update()
    {
        //_movementStateMachine?.Update();
        SkillStateMachine?.Update();
    }

    private void FixedUpdate()
    {
        //_movementStateMachine?.FixedUpdate();
        SkillStateMachine?.FixedUpdate();
    }

    private void OnEnable()
    {
        GameManager.OnSave += SavePlayerData;
        GameManager.OnLoad += LoadPlayerData;
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
            Destroy(gameObject);
    }

    public void CheckIfHitIsAvailable(BulletPro.Bullet bullet, Vector3 position)
    {
        int damageAmount = bullet.moduleParameters.GetInt("Damage");
        Damage(damageAmount);

        //TODO Need to create a IFrame
        bullet.Die();
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
                hp = _currentHealth,
            }
        };
        return data;
    }

    public void LoadPlayerData(SaveData saveData)
    {
        _currentHealth = saveData.PlayerData.hp;
        UIManager.Instance.ChangeCurrentHealth(_currentHealth);
    }
}
