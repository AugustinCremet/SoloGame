using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] GameObject _HPOdd;
    [SerializeField] GameObject _HPEven;
    private int _maxHealth;
    private int _currentHealth;

    private void Awake()
    {
        _currentHealth = 10;
    }
    public void ChangeCurrentHealth(int currentHealth)
    {
        int healthToRemove = _currentHealth - currentHealth;
        
        if(healthToRemove > 0)
        {
            for(int i = 0;  i < healthToRemove; i++)
            {
                if(_currentHealth % 2 == 0)
                {
                    _HPEven.transform.GetChild(_currentHealth / 2 - 1).GetComponent<Image>().enabled = false;
                }
                else
                {
                    _HPOdd.transform.GetChild(_currentHealth / 2).GetComponent<Image>().enabled = false;
                }
                _currentHealth--;
            }
        }
        else
        {
            healthToRemove *= -1;
            for (int i = 0; i < healthToRemove; i++)
            {
                if (_currentHealth % 2 != 0)
                {
                    _HPEven.transform.GetChild(_currentHealth / 2).GetComponent<Image>().enabled = true;
                }
                else
                {
                    _HPOdd.transform.GetChild(_currentHealth / 2).GetComponent<Image>().enabled = true;
                }
                _currentHealth++;
            }
        }
        
    }

    public void ChangeMaxHealth(int maxHealth)
    {
        _maxHealth = maxHealth;
    }
}
