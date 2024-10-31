using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Camera cam;
    Vector3 mousePos;
    Rigidbody2D rb;
    Collider2D col;
    public string tagToIgnore;
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] int dmg = 10;
    
    
    void Awake()
    {
        cam = FindAnyObjectByType<Camera>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        rb.velocity = new Vector2 (direction.x, direction.y).normalized * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>() != null)
            return;

        if (collision.gameObject.GetComponent<IDamageable>() != null &&
            !collision.gameObject.CompareTag(tagToIgnore))
        {
            collision.gameObject.GetComponent<IDamageable>().Damage(dmg);
            Destroy(gameObject);
        }
        else if (!collision.gameObject.CompareTag(tagToIgnore))
        {
            Debug.Log(collision.gameObject.name);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
}
