using UnityEngine;

public class SwarmWander : MonoBehaviour
{
    [SerializeField] float _radius = 1f;
    [SerializeField] float _speed = 1f;
    private Vector3 _initialPosition;
    private float _offsetX;
    private float _offsetY;
    
    void Awake()
    {
        _offsetX = Random.value * 100f;
        _offsetY = Random.value * 100f;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Mathf.PerlinNoise(Time.time * _speed + _offsetX, 0f) - 0.5f;
        float y = Mathf.PerlinNoise(0f, Time.time * _speed + _offsetY) - 0.5f;

        Vector3 offset = new Vector3(x, y, 0f) * _radius;
        transform.localPosition = _initialPosition + offset;
    }

    public void Init(Vector3 initialPosition)
    {
        transform.localPosition = initialPosition;
        _initialPosition = initialPosition;
    }

    // Put the logic in the parent so enemy/AI stay with the behavior graph agent
    public void CheckIfHitIsAvailable(BulletPro.Bullet bullet, Vector3 position)
    {
        GetComponentInParent<Enemy>().CheckIfHitIsAvailable(bullet, position);
    }
}
