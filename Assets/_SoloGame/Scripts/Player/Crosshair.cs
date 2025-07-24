using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField] Image _crossHair;
    private float _cooldownDuration = 1f;
    private float _cooldownTimer = 0f;

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
