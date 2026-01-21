using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using static System.TimeZoneInfo;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] private CinemachineConfiner2D _confiner;
    [SerializeField] private CinemachineGroupFraming _groupFraming;
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private CinemachinePositionComposer _positionComposer;

    private ScreenRadialFade _screenRadialFade;
    private Transform _player;
    private Transform _lastVisibleWorldPos;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _screenRadialFade = FindAnyObjectByType<ScreenRadialFade>();
        _player = FindAnyObjectByType<Player>().transform;
    }

    private void LateUpdate()
    {
        Vector3 vp = Camera.main.WorldToViewportPoint(_player.position);

        bool visible =
            vp.z > 0f &&
            vp.x >= 0f && vp.x <= 1f &&
            vp.y >= 0f && vp.y <= 1f;

        if (visible)
        {
            _lastVisibleWorldPos = _player;
        }
    }

    public void SetBoundary(CompositeCollider2D newBoundary)
    {
        _confiner.BoundingShape2D = newBoundary;
    }

    public void SetOrthoSize(float newSize)
    {
        _groupFraming.OrthoSizeRange = new Vector2(newSize, newSize);
    }

    public IEnumerator SetFixedRoomBoundary(CompositeCollider2D roomCollider, Player player)
    {
        Vector3 playerPos = player.transform.position;
        player.transform.GetComponent<PlayerController>().SwitchActionMap(InputMode.Loading);
        yield return _screenRadialFade.FadeOut(playerPos);

        Bounds bounds = roomCollider.bounds;

        Vector3 center = bounds.center;
        _camera.ForceCameraPosition(center, Quaternion.identity);

        float roomHeight = bounds.size.y;
        _camera.Lens.OrthographicSize = roomHeight / 2f;

        _confiner.BoundingShape2D = roomCollider;
        _confiner.InvalidateBoundingShapeCache();

        playerPos = player.transform.position;
        yield return null; // Wait a frame to ensure camera position is updated
        yield return _screenRadialFade.FadeIn(playerPos);
        player.transform.GetComponent<PlayerController>().SwitchActionMap(InputMode.Gameplay);
    }

    public void DampToNewRoom(CompositeCollider2D roomCollider, Player player)
    {
        Vector3 playerPos = player.transform.position;
        player.transform.GetComponent<PlayerController>().SwitchActionMap(InputMode.Loading);

        Bounds bounds = roomCollider.bounds;
        Vector3 center = bounds.center;
        GameObject originalTarget = _camera.Follow.gameObject;


        _camera.Follow = _lastVisibleWorldPos;
        Transform anchor = roomCollider.transform;
        _camera.Follow = anchor;
        float roomHeight = bounds.size.y;
        _camera.Lens.OrthographicSize = roomHeight / 2f;

        _confiner.BoundingShape2D = roomCollider;
        _confiner.InvalidateBoundingShapeCache();

        playerPos = player.transform.position;
        player.transform.GetComponent<PlayerController>().SwitchActionMap(InputMode.Gameplay);
        _camera.Follow = originalTarget.transform;
    }

    private IEnumerator ZoomIn(float originalSize, float zoomAmount = 0.6f)
    {
        float t = 0f;
        float newSize = originalSize * zoomAmount;

        while (t < 1f)
        {
            t += Time.deltaTime;
            _camera.Lens.OrthographicSize = Mathf.Lerp(_camera.Lens.OrthographicSize, newSize, t);
            yield return null;
        }
    }

    private IEnumerator ZoomOut(float zoomAmount, float originalSize)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            _camera.Lens.OrthographicSize = Mathf.Lerp(_camera.Lens.OrthographicSize, zoomAmount, t);
            yield return null;
        }
    }
}
