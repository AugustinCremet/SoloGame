using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField] Image _crossHair;
    [SerializeField] float _distanceToPlayerUsingController = 5f;
    private float _cooldownDuration = 1f;
    private float _cooldownTimer = 0f;
    private Camera _cam;
    private Vector2 _lastStickDir = Vector2.right;

    private void Awake()
    {
        _cam = FindAnyObjectByType<Camera>();
    }
    private void Update()
    {
        AdjustCrosshair();
    }

    // Adjustement for any aspect ratio (except free)
    private void AdjustCrosshair()
    {
        Vector2 crosshairMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        transform.position += new Vector3(crosshairMovement.x, crosshairMovement.y, 0f);

        Vector3 crosshairScreenPos = _cam.WorldToScreenPoint(transform.position);
        crosshairScreenPos.x = Mathf.Clamp(crosshairScreenPos.x, 0f, Screen.width);
        crosshairScreenPos.y = Mathf.Clamp(crosshairScreenPos.y, 0f, Screen.height);

        Vector3 clampedWorldPos = _cam.ScreenToWorldPoint(crosshairScreenPos);
        clampedWorldPos.z = 0f;

        transform.position = clampedWorldPos;
    }

    public void SetAimDirectionFromController(Vector2 aimDir, Player player)
    {
        if(aimDir.magnitude < 0.1f)
        {
            aimDir = _lastStickDir;
        }
        else
        {
            _lastStickDir = aimDir;
        }

        transform.position = player.transform.position + (Vector3)_lastStickDir * _distanceToPlayerUsingController;
    }

    public void StartCooldown(float cooldownDuration)
    {
        _cooldownDuration = cooldownDuration;
        _crossHair.fillAmount = _cooldownTimer;
        StartCoroutine(FillUpCrosshairCR());
    }

    private IEnumerator FillUpCrosshairCR()
    {
        while(_cooldownTimer < _cooldownDuration)
        {
            _cooldownTimer += Time.deltaTime;
            _crossHair.fillAmount = _cooldownTimer / _cooldownDuration;
            yield return null;
        }

        _cooldownTimer = 0f;
    }
}
