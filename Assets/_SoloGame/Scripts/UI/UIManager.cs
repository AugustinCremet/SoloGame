using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] HealthBar _healthBar;
    [SerializeField] GooBar _gooBar;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void ChangeCurrentHealth(int currentHealth)
    {
        _healthBar.ChangeCurrentHealth(currentHealth);
    }

    public void ChangeMaxHealth(float maxHealth)
    {
        _healthBar.ChangeMaxHealth(maxHealth);
    }

    public void ChangeCurrentGoo(float currentGoo)
    {
        _gooBar.ChangeCurrentGoo(currentGoo);
    }

    public void ChangeMaxGoo(float maxGoo)
    {
        _gooBar.ChangeMaxGoo(maxGoo);
    }
}
