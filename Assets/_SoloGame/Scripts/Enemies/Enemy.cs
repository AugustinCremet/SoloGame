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
    private string _tagSelf;
    [SerializeField] string _uniqueID;

    public bool IsAttacking { get; private set; } = false;
    public bool IsAIActive { get; private set; } = false;

    public static event Action<Enemy> OnEnemyDeath;

    private void Awake()
    {
        _tagSelf = gameObject.tag;
        _bulletEmitter = GetComponent<BulletEmitter>();
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
        //if(!_canAttack)
        //{
        //    _currentWaitingTime += Time.deltaTime;
        //    if (_currentWaitingTime > 0.3f)
        //    {
        //        _canAttack = true;
        //        _currentWaitingTime = 0f;
        //    }
        //}
    }

    public void CheckIfHitIsAvailable(BulletPro.Bullet bullet, Vector3 position)
    {
        Damage(bullet.moduleParameters.GetInt("Damage"));
    }
    public void Damage(int dmgAmount)
    {
        _hp -= dmgAmount;

        if(_hp <= 0)
        {
            GameManager.Instance.MarkEnemyTempDead(_uniqueID);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        OnEnemyDeath?.Invoke(this);
    }

    public bool Attack()
    {
        return true;
    }

    public void StopAttack()
    {
        _bulletEmitter?.Stop(PlayOptions.RootOnly);
    }

    IEnumerator AttackRoutine()
    {
        IsAttacking = true;
        //AudioSource.PlayClipAtPoint(_soundClip, transform.position);

        //yield return new WaitForSeconds(_soundClip.length);
        yield return null;
        _bulletEmitter.Play();
        IsAttacking = false;
    }

    public void StartAttack()
    {
        if(!IsAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }
}
