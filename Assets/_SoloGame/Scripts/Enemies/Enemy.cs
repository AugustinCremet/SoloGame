using System;
using System.Collections;
using BulletPro;
using Unity.Behavior;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SoundHandler))]
public class Enemy : MonoBehaviour, IDamageable, IEnemyAttack, IUniqueIdentifier
{
    [SerializeField] int _hp = 100;
    private BulletEmitter _bulletEmitter;
    private bool _isOnAttackCooldown = false;
    [SerializeField] int _damageOnTouch = 1;
    [SerializeField] float _pushAmount = 0f;
    protected virtual bool _canBePermaDead => false;
    public bool IsAIActive { get; private set; } = false;
    [SerializeField] string _uniqueID;
    public string UniqueID { get => _uniqueID; set => _uniqueID = value; }

    public static event Action<Enemy> OnEnemyDeath;
    private SoundHandler _soundHandler;

    private void Awake()
    {
        _bulletEmitter = GetComponent<BulletEmitter>();
        _soundHandler = GetComponent<SoundHandler>();
        if(TryGetComponent<NavMeshAgent>(out var agent))
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            agent.radius = 0.5f;
        }
    }

    private void Start()
    {
        if (GameManager.Instance.IsEnemyTempDead(_uniqueID))
        {
            Debug.Log("Start destroy");
            Destroy(gameObject);
        }
    }

    // Old method, changed for a global one
//    [ContextMenu("Assign Unique ID")]
//    private void AssignUniqueIDInEditor()
//    {
//#if UNITY_EDITOR
//        if (string.IsNullOrEmpty(_uniqueID))
//        {
//            Undo.RecordObject(this, "Assign Unique ID");
//            _uniqueID = Guid.NewGuid().ToString();
//            EditorUtility.SetDirty(this); // Mark the object as changed so Unity saves it
//            Debug.Log($"Assigned new ID: {_uniqueID}", this);
//        }
//        else
//        {
//            Debug.LogWarning($"ID already assigned: {_uniqueID}", this);
//        }
//#endif
//    }

    public void SetAI(bool isActive)
    {
        IsAIActive = isActive;
    }

    public void StartAttackCooldown(float cooldown)
    {
        StartCoroutine(AttackCooddownCR(cooldown));
    }

    public IEnumerator AttackCooddownCR(float cooldown)
    {
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
        Damage(bullet.moduleParameters.GetInt("Damage"));
    }
    public void Damage(int dmgAmount, Vector2? hitLocation = null, float force = 0f)
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
        Debug.Log("Destroy enemy");
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

    public void StopAttack(float cooldown)
    {
        _bulletEmitter?.Stop(PlayOptions.RootOnly);
        StartAttackCooldown(cooldown);
    }

    public void StartAttack()
    {
        _bulletEmitter.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<IDamageable>(out var damageable) && collision.CompareTag("Player"))
        {
            damageable.Damage(_damageOnTouch, transform.position, _pushAmount);
        }
    }
}
