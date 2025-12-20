using UnityEngine;

public class LobEffect : MonoBehaviour
{
    [SerializeField] Sprite _spriteShadow;
    private GameObject _shadow;

    private Vector3 _startPos;
    private float _travelDistance;
    [SerializeField] float _travelSpeed = 1f;
    private Vector3 _direction;
    private bool _isMoving;
    [SerializeField] float _maxHeight = 3f;
    [SerializeField] float _minScale = 1f;
    [SerializeField] float _maxScale = 3f;

    private CircleCollider2D _collider;

    private void Awake()
    {
        _startPos = transform.position;
        _collider = GetComponent<CircleCollider2D>();
    }

    private void Initialize()
    {
        _shadow = new GameObject("Shadow");
        var renderer = _shadow.AddComponent<SpriteRenderer>();
        renderer.sprite = _spriteShadow;
        renderer.sortingOrder = -1;
        renderer.sortingLayerName = "Bullet";
        renderer.color = new Color(0f, 0f, 0f, 0.6f);
        _shadow.transform.position = transform.position;
    }

    private void Update()
    {
        if(_isMoving)
        {
            MoveObject();
            MoveShadow();
        }
    }

    private void MoveObject()
    {
        float currentDistance = Vector2.Distance(_shadow.transform.position, _startPos);
        float tHeight = Mathf.Clamp01(currentDistance / _travelDistance);
        float groundFactor = Mathf.Abs(Vector2.Dot(_direction.normalized, Vector2.right));
        groundFactor = Mathf.Clamp(groundFactor, 0.25f, 1.0f);

        // Parabolic height
        float height = 4 * _maxHeight * tHeight * groundFactor * (1 - tHeight);

        float currentHeight = transform.position.y - _shadow.transform.position.y;
        float tScale = Mathf.Clamp01(currentHeight / _maxHeight);
        float scale = Mathf.Lerp(_minScale, _maxScale, tScale);

        // Position above the shadow
        Vector3 pos = _shadow.transform.position + Vector3.up * height;
        transform.position = pos;
        transform.localScale = Vector3.one * scale;
        _shadow.transform.localScale = Vector3.one * scale;

        if(tHeight >= 1f)
        {
            _isMoving = false;
            _collider.enabled = true;
            Destroy(_shadow);
        }
    }

    private void MoveShadow()
    {
        _shadow.transform.position += _direction * _travelSpeed * Time.deltaTime;

    }

    public void StartMoving(Vector3 endPos, float duration)
    {
        Initialize();
        _collider.enabled = false;
        _travelDistance = (endPos - transform.position).magnitude;
        _direction = (endPos - transform.position).normalized;
        _travelSpeed = _travelDistance / duration;
        _isMoving = true;
    }
}
