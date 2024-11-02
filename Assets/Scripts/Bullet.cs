using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 mousePos;
    Rigidbody2D rb;
    Collider2D col;
    Element element;
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] int dmg = 10;
    
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        
    }

    public void InitializeBullet(Vector3 direction, Element element)
    {
        this.element = element;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * moveSpeed;

        //Temp for prototype visual
        switch (element.elementType)
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

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.GetComponent<IDamageable>() != null)
        {
            collision.gameObject.GetComponent<IDamageable>().Damage(element, dmg);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
