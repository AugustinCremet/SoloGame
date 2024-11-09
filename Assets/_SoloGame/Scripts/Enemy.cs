using BehaviorTree;
using BulletPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] int _hp = 100;
    [SerializeField] GameObject _bullet;
    BulletEmitter _bulletEmitter;
    //[SerializeField] ElementType elementType;
    //Element currentElement;
    string _tagSelf;
    float _currentWaitingTime = 0f;
    bool _canAttack = true;

    private void Awake()
    {
        _tagSelf = gameObject.tag;
        _bulletEmitter = GetComponent<BulletEmitter>();
    }

    private void Start()
    {
        //currentElement = ElementManager.Instance.GetElementByType(elementType);

        //Temp for prototype visual
        //switch(elementType)
        //{
        //    case ElementType.Fire:
        //        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        //        break;
        //    case ElementType.Grass:
        //        gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        //        break;
        //    case ElementType.Water:
        //        gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        //        break;
        //    default:
        //        break;
        //}
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
    public void Damage(int dmgAmount)
    {
        //int damageAfterElement = (int)currentElement.CalculateDamageFrom(element, dmgAmount);
        //hp -= damageAfterElement;

        if(_hp <= 0)
            Destroy(gameObject);
    }

    public void Attack()
    {
        if (_canAttack)
        {
            //canAttack = false;
            //GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            //Physics2D.IgnoreCollision(newBullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            //Vector3 direction = GameObject.FindWithTag("Player").transform.position - transform.position;
            //newBullet.GetComponent<Bullet>().InitializeBullet(direction, currentElement);
            //_bulletEmitter.Play();
        }
    }
}
