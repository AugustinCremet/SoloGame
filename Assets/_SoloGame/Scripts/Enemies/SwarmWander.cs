using UnityEngine;

public class SwarmWander : MonoBehaviour
{
    private float _radius = 1f;
    private float _speed = 1f;
    private Vector3 _initialPosition;
    private float _offsetX;
    private float _offsetY;
    
    void Awake()
    {
        _offsetX = Random.value * 100f;
        _offsetY = Random.value * 100f;
        _initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Mathf.PerlinNoise(Time.time * _speed + _offsetX, 0f) - 0.5f;
        float y = Mathf.PerlinNoise(0f, Time.time * _speed + _offsetY) - 0.5f;

        Vector3 offset = new Vector3(x, y, 0f) * _radius;
        transform.localPosition = _initialPosition + offset;
    }

    public void Init(float radius, float speed)
    {
        _radius = radius;
        _speed = speed;
    }
}
