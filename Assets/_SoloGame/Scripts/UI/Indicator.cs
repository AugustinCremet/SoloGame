using Unity.Cinemachine.Samples;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    [SerializeField] GameObject _indicator;
    private GameObject _target;
    private Camera _camera;

    private SpriteRenderer _spriteRenderer;
    private float _spriteWidth;
    private float _spriteHeight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _spriteRenderer = _indicator.GetComponent<SpriteRenderer>();
        _indicator.SetActive(false);

        Bounds bounds = _spriteRenderer.bounds;
        _spriteHeight = bounds.size.y / 2f;
        _spriteWidth = bounds.size.x / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = _camera.WorldToViewportPoint(transform.position);
        bool isOffScreen = screenPos.x <= 0f || screenPos.x >= 1f || screenPos.y <= 0f || screenPos.y >= 1f;
        
        if(isOffScreen)
        {
            _indicator.SetActive(true);
            Vector3 spriteSizeInViewport = _camera.WorldToViewportPoint(new Vector3(_spriteWidth, _spriteHeight, 0f)) - _camera.WorldToViewportPoint(Vector3.zero);

            screenPos.x = Mathf.Clamp(screenPos.x, spriteSizeInViewport.x, 1f - spriteSizeInViewport.x);
            screenPos.y = Mathf.Clamp(screenPos.y, spriteSizeInViewport.y, 1f - spriteSizeInViewport.y);

            Vector3 worldPos = _camera.ViewportToWorldPoint(screenPos);
            worldPos.z = 0f;
            _indicator.transform.position = worldPos;

            // Rotate object if need be
            //Vector2 direction = _target.transform.position - _indicator.transform.position;
            //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //_indicator.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else
        {
            _indicator.SetActive(false);
        }
    }
}
