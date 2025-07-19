using UnityEngine;
using UnityEngine.AI;

public class SlimeBall : MonoBehaviour, ICollectable
{
    private Rigidbody2D _rb;
    private NavMeshAgent _agent;
    private Transform _playerTransform;
    [SerializeField] LayerMask _obstacleLayer;
    [SerializeField] float _attractionRange = 5f;
    private float _raycastCurrentTime = 0f;
    private float _raycastTime = 0.2f;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.enabled = false;
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
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
        Vector2 directionToPlayer = _playerTransform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if(distanceToPlayer <= _attractionRange)
        {
            bool isPlayerInSight = UtilityFunctions.IsPlayerInSight(transform.position, _playerTransform);

            if (isPlayerInSight)
            {
                _agent.enabled = true;
                _agent.SetDestination(_playerTransform.position);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, _attractionRange);
    }
}
