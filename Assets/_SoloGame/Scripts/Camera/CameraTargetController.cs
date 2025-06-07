using Unity.Cinemachine;
using UnityEngine;

public class CameraTargetController : MonoBehaviour
{
    private Transform _player;
    [SerializeField] float _maxOffsetDistance = 3f;
    [SerializeField] float _minOffsetDistance = 0.5f;
    private bool _useMouse = true;
    private CinemachineTargetGroup.Target _cameraTarget;
    private float _originalWeight;

    private void Awake()
    {
        _player = transform.parent;

        CinemachineTargetGroup targetGroup = FindAnyObjectByType<CinemachineTargetGroup>();
        foreach (var target in targetGroup.Targets)
        {
            if (target.Object.transform == transform)
            {
                _cameraTarget = target;
                _originalWeight = target.Weight;
            }
        }
    }

    void LateUpdate()
    {
        Vector3 targetPosition = _player.position;

        if (_useMouse)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            Vector3 dir = mouseWorldPos - _player.position;
            float t = Mathf.InverseLerp(_minOffsetDistance, _maxOffsetDistance, dir.magnitude);
            float weight = Mathf.SmoothStep(0f, _originalWeight, t);

            // Clamp direction to max offset
            if (dir.magnitude > _maxOffsetDistance)
            {
                dir = dir.normalized * _maxOffsetDistance;
            }

            if(dir.magnitude > _minOffsetDistance)
            {
                _cameraTarget.Weight = weight;
            }
            else if(dir.magnitude < _minOffsetDistance)
            {
                _cameraTarget.Weight = weight;
            }

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
    }
}
