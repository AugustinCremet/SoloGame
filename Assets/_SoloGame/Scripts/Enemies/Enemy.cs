using BehaviorTree;
using BulletPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] int _hp = 100;
    [SerializeField] GameObject _laserSight;
    private BulletEmitter _bulletEmitter;
    private string _tagSelf;
    private float _currentWaitingTime = 0f;
    private bool _canAttack = true;

    private void Awake()
    {
        _tagSelf = gameObject.tag;
        _bulletEmitter = GetComponent<BulletEmitter>();
    }

    private void Update()
    {
        if(!_canAttack)
        {
            _currentWaitingTime += Time.deltaTime;
            if (_currentWaitingTime > 0.3f)
            {
                _canAttack = true;
                _currentWaitingTime = 0f;
            }
        }
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
        if (_bulletEmitter.isPlaying == false)
        {
            _bulletEmitter.Play();
            //TODO temp for laserSight
            Instantiate(_laserSight, transform);
            return false;
        }
        else
        {
            return true;
        }
    }

    public void StopAttack()
    {
        _bulletEmitter?.Stop(PlayOptions.RootOnly);
        Debug.Log("Stop Attack");
    }
}
