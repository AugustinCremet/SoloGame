using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletTimeBar : MonoBehaviour
{
    Slider _slider;
    float _maxBulletTime = 100f;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        ChangeCurrentBulletTime(_maxBulletTime);
    }
    public void ChangeCurrentBulletTime(float currentBulletTime)
    {
        float fillAmount = currentBulletTime / _maxBulletTime;
        _slider.value = fillAmount;
    }

    public void ChangeMaxBulletTime(float maxBulletTime)
    {
        _maxBulletTime = maxBulletTime;
    }
}
