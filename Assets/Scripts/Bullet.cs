using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Camera cam;
    Vector3 mousePos;
    Rigidbody2D rb;
    Collider2D col;
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] int dmg = 10;

    void Start()
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
        if (collision.gameObject.GetComponent<IDamageable>() != null)
        {
            collision.GetComponent<IDamageable>().Damage(dmg);
        }
    }
}
