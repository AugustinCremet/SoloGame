using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LiquidBar : MonoBehaviour
{
    private Slider _slider;
    private Player _player;

    private float _currentValue = 100f;
    private float _currentTarget = 100f;
    private float _drainAmount = 50f;

    private Coroutine _bounceCoroutine;
    private bool _isRemovingLiquid;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void Start()
    {
        _player = FindFirstObjectByType<Player>();
    }
    public void ChangeValue(float targetValue)
    {
        if(targetValue < _currentValue)
        {
            SetValueWithBounce(targetValue);
        }
        else
        {
            _slider.value = targetValue;
        }
    }

    private void Update()
    {
        if(_isRemovingLiquid)
        {
            RemoveLiquidPeriodicly();
        }
    }

    public void StartRemovingLiquid(Player player)
    {
        _isRemovingLiquid = true;
        _player = player;
    }

    public void StopRemovingLiquid()
    {
        _isRemovingLiquid = false;
        if(_slider.value > 0f)
        {
            SetValueWithBounce(_slider.value);
        }
    }

    private void RemoveLiquidPeriodicly()
    {
        bool flowControl = IsLiquidTotallyGone();

        if (!flowControl)
        {
            return;
        }

        float amountToRemove = _drainAmount * Time.deltaTime;
        _currentValue = _slider.value;
        _slider.value = _currentValue - amountToRemove;
    }

    private bool IsLiquidTotallyGone()
    {
        if (_slider.value <= 0f)
        {
            _player.RemoveLiquid();
            gameObject.SetActive(false);
            return false;
        }

        return true;
    }

    public void SetValueWithBounce(float target)
    {
        if (_bounceCoroutine != null)
            StopCoroutine(_bounceCoroutine);

        _bounceCoroutine = StartCoroutine(BounceToValueDynamic(target));
    }

    private IEnumerator BounceToValueDynamic(float target)
    {
        float overshoot1 = -7f; // first big dip below
        float overshoot2 = 5f;   // small bump above
        float overshoot3 = -3f;  // smaller dip
        float overshoot4 = 1f;   // tiny final bump

        float[] overshoots = { overshoot1, overshoot2, overshoot3, overshoot4 };

        float duration = 0.15f;

        foreach (float o in overshoots)
        {
            float start = _slider.value;
            float next = target + o;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                _slider.value = Mathf.Lerp(start, next, elapsed / duration);
                yield return null;
            }

            _slider.value = next;
            duration *= 0.75f; // make each bounce faster
        }

        // Final settle on target
        float settleTime = 0.15f;
        float startFinal = _slider.value;
        float elapsedFinal = 0f;
        while (elapsedFinal < settleTime)
        {
            elapsedFinal += Time.deltaTime;
            _slider.value = Mathf.Lerp(startFinal, target, elapsedFinal / settleTime);
            yield return null;
        }

        _slider.value = target;
        _bounceCoroutine = null;
        IsLiquidTotallyGone();
    }
}
