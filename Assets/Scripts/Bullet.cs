using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 mousePos;
    Rigidbody2D rb;
    Collider2D col;
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] int dmg = 10;
    
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        
    }

    public void SetBulletDirection(Vector3 direction)
    {
        rb.velocity = new Vector2(direction.x, direction.y).normalized * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.GetComponent<IDamageable>() != null)
        {
            collision.gameObject.GetComponent<IDamageable>().Damage(dmg);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
