using UnityEngine;

public class CameraTargetController : MonoBehaviour
{
    private Transform _player;
    [SerializeField] float _maxOffsetDistance = 3f;
    private bool _useMouse = true;

    private void Awake()
    {
        _player = transform.parent;
    }

    void LateUpdate()
    {
        Vector3 targetPosition = _player.position;

        if (_useMouse)
        {
            // Get mouse position in world space
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f; // ensure Z is ignored in 2D

            // Direction from player to mouse
            Vector3 dir = mouseWorldPos - _player.position;

            // Clamp direction to max offset
            if (dir.magnitude > _maxOffsetDistance)
                dir = dir.normalized * _maxOffsetDistance;

            targetPosition += dir;
        }
        else
        {
            float x = Input.GetAxis("RightStickHorizontal");
            float y = Input.GetAxis("RightStickVertical");

            Vector2 input = new Vector2(x, y);
            if (input.sqrMagnitude > 1f)
                input.Normalize();

            targetPosition += (Vector3)(input * _maxOffsetDistance);
        }

        transform.position = targetPosition;
        //transform.position = new Vector3(3f, 3f, 0f);
    }
}
