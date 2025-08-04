using BehaviorTree;
using BulletPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable, IEnemyAttack
{
    [SerializeField] int _hp = 100;
    [SerializeField] GameObject _laserSight;
    [SerializeField] AudioClip _soundClip;
    private BulletEmitter _bulletEmitter;
    [SerializeField] string _uniqueID;
    private float _shootingCurrentTime;
    private bool _isShootingTimerActive;
    private bool _isOnAttackCooldown = false;
    protected virtual bool _canBePermaDead => false;

    public bool IsAttacking { get; private set; } = false;
    public bool IsAIActive { get; private set; } = false;

    public static event Action<Enemy> OnEnemyDeath;
    private SoundHandler _soundHandler;

    private void Awake()
    {
        _bulletEmitter = GetComponent<BulletEmitter>();
        _soundHandler = GetComponent<SoundHandler>();
        Instantiate(_laserSight, transform);
    }

    private void Start()
    {
        if (GameManager.Instance.IsEnemyTempDead(_uniqueID))
            Destroy(gameObject);
    }

    [ContextMenu("Assign Unique ID")]
    private void AssignUniqueIDInEditor()
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(_uniqueID))
        {
            Undo.RecordObject(this, "Assign Unique ID");
            _uniqueID = Guid.NewGuid().ToString();
            EditorUtility.SetDirty(this); // Mark the object as changed so Unity saves it
            Debug.Log($"Assigned new ID: {_uniqueID}", this);
        }
        else
        {
            Debug.LogWarning($"ID already assigned: {_uniqueID}", this);
        }
#endif
    }

    public void SetAI(bool isActive)
    {
        IsAIActive = isActive;
    }

    private void Update()
    {
        if (_isShootingTimerActive)
        {
            ShootingTimer();
        }
    }

    public void StartShootingTimer()
    {
        _isShootingTimerActive = true;
    }

    private void ShootingTimer()
    {
        _shootingCurrentTime += Time.deltaTime;
    }

    public bool IsShootingTimeDone(float shootingThreshold)
    {
        bool isShootingTimeDone = false;

        if(_shootingCurrentTime >= shootingThreshold)
        {
            isShootingTimeDone = true;
            _isShootingTimerActive = false;
            _shootingCurrentTime = 0f;
        }
        else
        {
            isShootingTimeDone = false;
        }
        

        return isShootingTimeDone;
    }

    public void StartAttackCooldown()
    {
        StartCoroutine(AttackCooddownCR());
    }

    public IEnumerator AttackCooddownCR()
    {
        float cooldown = 2f;
        _isOnAttackCooldown = true;
        while(cooldown >= 0f)
        {
            cooldown -= Time.deltaTime;
            yield return null;
        }
        _isOnAttackCooldown = false;
    }

    public bool IsAttackCooldown()
    {
        return _isOnAttackCooldown;
    }

    public void CheckIfHitIsAvailable(BulletPro.Bullet bullet, Vector3 position)
    {
        Debug.Log("Check hit");
        Damage(bullet.moduleParameters.GetInt("Damage"));
        Debug.Log("Check 2");
    }
    public void Damage(int dmgAmount)
    {
        _hp -= dmgAmount;
        _soundHandler.Play(ESoundType.OnHit);

        if(_hp <= 0)
        {
            _soundHandler.Play(ESoundType.OnDeath);
            GameManager.Instance.MarkEnemyTempDead(_uniqueID, false);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        OnEnemyDeath?.Invoke(this);
    }

    public bool IsEmitterPlaying()
    {
        return _bulletEmitter.isPlaying;
    }

    public bool Attack()
    {
        return true;
    }

    public void StopAttack()
    {
        _bulletEmitter?.Stop(PlayOptions.RootOnly);
    }

    public void StartAttack()
    {
        StartAttackCooldown();
        _bulletEmitter.Play();
    }
}
