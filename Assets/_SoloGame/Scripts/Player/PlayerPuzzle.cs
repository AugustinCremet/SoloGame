using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPuzzle : MonoBehaviour
{
    private Vector2 _horizontalMovement = Vector2.zero;
    private Rigidbody2D _rb;
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] GameObject _gooPrefab;
    private float _gooCD = 0.05f;
    private float _gooCurrentCD;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_horizontalMovement.x != 0f || _horizontalMovement.y != 0f)
        {
            //HandleGoo();
        }
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _horizontalMovement * _moveSpeed;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        _horizontalMovement = context.ReadValue<Vector2>();
        if (_horizontalMovement.x != 0f)
        {
            if (_horizontalMovement.x > 0f)
            {
                transform.eulerAngles = new Vector3(0f, 0f, -90f);
            }
            else
            {
                transform.eulerAngles = new Vector3(0f, 0f, 90f);
            }
        }
        else if (_horizontalMovement.y != 0f)
        {
            if (_horizontalMovement.y > 0f)
            {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
            else
            {
                transform.eulerAngles = new Vector3(0f, 0f, 180f);
            }
        }
    }

    public void HandleGoo()
    {
        //GetComponentInChildren<ParticleSystem>().Play();
        if (_gooCurrentCD <= 0)
        {
            GameObject instance = Instantiate(_gooPrefab, transform.position, Quaternion.identity);
            _gooCurrentCD = _gooCD;
            //Destroy(instance, .8f);
        }
        else
        {
            _gooCurrentCD -= Time.deltaTime;
        }
    }
}
