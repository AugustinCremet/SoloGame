using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] int hp = 100;
    [SerializeField] GameObject bullet;
    [SerializeField] ElementType elementType;
    Element currentElement;
    string tagSelf;
    float currentWaitingTime = 0f;
    bool canAttack = true;

    private void Awake()
    {
        tagSelf = gameObject.tag;
    }

    private void Start()
    {
        currentElement = ElementManager.instance.GetElementByType(elementType);

        //Temp for prototype visual
        switch(elementType)
        {
            case ElementType.Fire:
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case ElementType.Grass:
                gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case ElementType.Water:
                gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if(!canAttack)
        {
            currentWaitingTime += Time.deltaTime;
            if (currentWaitingTime > 0.3f)
            {
                canAttack = true;
                currentWaitingTime = 0f;
            }
        }
    }
    public void Damage(Element element, int dmgAmount)
    {
        int damageAfterElement = (int)currentElement.CalculateDamageFrom(element, dmgAmount);
        Debug.Log($"{dmgAmount} is now {damageAfterElement}");
        hp -= damageAfterElement;

        if(hp <= 0)
            Destroy(gameObject);
    }

    public void Attack()
    {
        if (canAttack)
        {
            canAttack = false;
            GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            Physics2D.IgnoreCollision(newBullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            Vector3 direction = GameObject.FindWithTag("Player").transform.position - transform.position;
            newBullet.GetComponent<Bullet>().InitializeBullet(direction, currentElement);
        }
    }
}
