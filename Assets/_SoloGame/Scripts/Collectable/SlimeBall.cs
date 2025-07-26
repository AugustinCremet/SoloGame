using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LobEffect))]
public class SlimeBall : MonoBehaviour, ICollectable
{
    private LobEffect _lobEffect;

    private Rigidbody2D _rb;
    private Transform _playerTransform;
    [SerializeField] LayerMask _obstacleLayer;
    [SerializeField] float _attractionRange = 5f;
    [SerializeField] float _attractionSpeed = 3f;
    private float _raycastCurrentTime = 0f;
    private float _raycastTime = 0.2f;

    private void OnEnable()
    {
        Player.OnSuction += OnSuction;
    }

    private void OnDisable()
    {
        Player.OnSuction -= OnSuction;
    }
    private void Awake()
    {
        _lobEffect = GetComponent<LobEffect>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Vector2 dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        float magnitude = Random.Range(1f, 2f);
        Vector2 force = dir * magnitude;
        _rb.AddForce(force, ForceMode2D.Impulse);

        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        _raycastCurrentTime += Time.deltaTime;

        if(_raycastCurrentTime >= _raycastTime)
        {
            _raycastCurrentTime = 0f;
            CheckForPlayer();
        }
    }

    public void OnCollect(GameObject collector)
    {
        collector.GetComponent<Player>().Heal(1);
        Destroy(gameObject);
    }

    private void CheckForPlayer()
    {
        Vector2 directionToPlayer = (_playerTransform.position - transform.position).normalized;
        float distanceToPlayer = (_playerTransform.position - transform.position).magnitude;

        if (distanceToPlayer <= _attractionRange)
        {
            bool isPlayerInSight = UtilityFunctions.IsPlayerInSight(transform.position, _playerTransform);

            if (isPlayerInSight)
            {
                transform.position += (Vector3)directionToPlayer * _attractionSpeed * Time.deltaTime;
            }
        }
    }

    private void OnSuction(float duration)
    {
        _lobEffect.StartMoving(_playerTransform.position, duration);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, _attractionRange);
    }
}
