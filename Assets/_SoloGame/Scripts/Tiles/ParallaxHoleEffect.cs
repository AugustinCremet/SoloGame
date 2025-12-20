using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class ParallaxHoleEffect : MonoBehaviour
{
    [SerializeField] TilemapCollider2D _wallCollider;
    [SerializeField] CompositeCollider2D _compositeCollider;
    [SerializeField] Transform _gridTransform;

    private Transform _playerTrans;
    private Bounds _bounds;

    private void Start()
    {
        _bounds = _wallCollider.bounds;
        _playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if(_playerTrans != null)
        {
            transform.position = new Vector3(-GetNormalizedPosition().x, -GetNormalizedPosition().y, transform.position.z) + _gridTransform.position;
        }
    }

    private Vector3 GetNormalizedPosition()
    {
        Vector3 offset = _playerTrans.position - _bounds.center;
        Vector3 extent = _compositeCollider.bounds.extents;

        return new Vector3(offset.x / extent.x, offset.y / extent.y);
    }
}
