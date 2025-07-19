using Unity.Cinemachine.Samples;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    [SerializeField] GameObject _indicator;
    private GameObject _target;
    private Camera _camera;

    public float screenEdgeBuffer = 50f; // Padding from edge (pixels)
    public float zDistanceFromCamera = 5f; //

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerScreenPos = _camera.WorldToScreenPoint(_target.transform.position);
        Vector3 slimeScreenPos = _camera.WorldToScreenPoint(transform.position);

        Vector3 direction = (slimeScreenPos - playerScreenPos).normalized;

        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        Vector3 slimeFromCenter = (slimeScreenPos - screenCenter).normalized;

        float maxX = Screen.width - screenEdgeBuffer;
        float maxY = Screen.height - screenEdgeBuffer;
        float minX = screenEdgeBuffer;
        float minY = screenEdgeBuffer;

        // Place indicator near screen edge in direction of slimeball
        Vector3 screenPos = screenCenter + slimeFromCenter * (Mathf.Min(screenCenter.x, screenCenter.y) - screenEdgeBuffer);
        screenPos.x = Mathf.Clamp(screenPos.x, minX, maxX);
        screenPos.y = Mathf.Clamp(screenPos.y, minY, maxY);
        screenPos.z = zDistanceFromCamera;

        // Convert back to world space
        Vector3 worldPos = _camera.ScreenToWorldPoint(screenPos);
        _indicator.transform.position = worldPos;

        // Rotate to face slimeball
        Vector3 toSlime = transform.position - _indicator.transform.position;
        float angle = Mathf.Atan2(toSlime.y, toSlime.x) * Mathf.Rad2Deg;
        _indicator.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
