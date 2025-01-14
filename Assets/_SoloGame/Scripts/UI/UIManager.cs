using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] HealthBar _healthBar;
    [SerializeField] BulletTimeBar _bulletTimeBar;

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

    public void ChangeCurrentHealth(float currentHealth)
    {
        _healthBar.ChangeCurrentHealth(currentHealth);
    }

    public void ChangeMaxHealth(float maxHealth)
    {
        _healthBar.ChangeMaxHealth(maxHealth);
    }

    public void ChangeCurrentBulletTime(float currentBulletTime)
    {
        _bulletTimeBar.ChangeCurrentBulletTime(currentBulletTime);
    }

    public void ChangeMaxBulletTime(float maxBulletTime)
    {
        _bulletTimeBar.ChangeMaxBulletTime(maxBulletTime);
    }
}
