using UnityEngine;
using System.Collections;

public class LiquidCooldown : MonoBehaviour
{
    [SerializeField] private RectTransform _liquidTransform;
    [SerializeField] private float _startY = -16f;
    [SerializeField] private float _endY = 0f;
    private Cooldown _cooldown;

    private bool _isActive;

    public void StartCooldown(Cooldown cooldown)
    {
        _cooldown = cooldown;
        _isActive = true;
    }

    private void Update()
    {
        if (_cooldown == null || !_isActive) return;

        float t = 1f - (_cooldown.RemainingTime / _cooldown.Duration);
        t = Mathf.Clamp01(t);
        float newY = Mathf.Lerp(_startY, _endY, t);

        Vector2 pos = _liquidTransform.anchoredPosition;
        pos.y = newY;
        _liquidTransform.anchoredPosition = pos;

        if (_cooldown.IsReady)
        {
            _isActive = false;
        }
    }
}
