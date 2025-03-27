using BehaviorTree;
using BulletPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable, IEnemyAttack
{
    [SerializeField] int _hp = 100;
    [SerializeField] GameObject _laserSight;
    [SerializeField] AudioClip _soundClip;
    private BulletEmitter _bulletEmitter;
    private string _tagSelf;
    private float _currentWaitingTime = 0f;
    private bool _canAttack = true;

    public bool IsAttacking { get; private set; } = false;

    private void Awake()
    {
        _tagSelf = gameObject.tag;
        _bulletEmitter = GetComponent<BulletEmitter>();
        Instantiate(_laserSight, transform);
        StartAttack();
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
        Debug.Log($"{dmgAmount} on {_hp} left");

        if(_hp <= 0)
            Destroy(gameObject);
    }

    public bool Attack()
    {
        return true;
    }

    public void StopAttack()
    {
        _bulletEmitter?.Stop(PlayOptions.RootOnly);
        Debug.Log("Stop Attack");
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
