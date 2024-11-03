using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Slider _slider;
    float _maxHealth = 100f;
    float _currentHealth;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _currentHealth = _maxHealth;
        ChangeCurrentHealth(_currentHealth);
    }
    public void ChangeCurrentHealth(float currentHealth)
    {
        float fillAmount = currentHealth / _maxHealth;
        _slider.value = fillAmount;
    }

    public void ChangeMaxHealth(float maxHealth)
    {
        _maxHealth = maxHealth;
    }
}
