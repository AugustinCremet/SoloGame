using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Camera cam;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] GameObject bullet;
    string tagSelf;

    Rigidbody2D rb;
    Vector2 horizontalMovement;


    // Start is called before the first frame update
    void Awake()
    {
        tagSelf = gameObject.tag;
        rb = GetComponent<Rigidbody2D>();
        cam = FindAnyObjectByType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = horizontalMovement * moveSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>();
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            Physics2D.IgnoreCollision(newBullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePos - transform.position;
            newBullet.GetComponent<Bullet>().SetBulletDirection(direction);
        }
    }
}
