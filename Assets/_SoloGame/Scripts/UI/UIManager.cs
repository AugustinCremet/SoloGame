using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] HealthBar _healthBar;
    [SerializeField] GooBar _gooBar;
    [SerializeField] LiquidBar _liquidBar;
    private GameObject _liquidBarGO;
    [SerializeField] LiquidCooldown _suctionCD;
    [SerializeField] TextMeshProUGUI _keyAmountText;

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

        _liquidBarGO = _liquidBar.gameObject;
        _liquidBarGO.SetActive(false);
    }

    public void ChangeCurrentHealth(int currentHealth)
    {
        _healthBar.ChangeCurrentHealth(currentHealth);
    }

    public void ChangeMaxHealth(int maxHealth)
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

    public void StartSkillCooldown(EPlayerSkill skill, Cooldown cooldown)
    {
        switch(skill)
        {
            case EPlayerSkill.Suction:
                _suctionCD.StartCooldown(cooldown);
                break;
            default:
                break;
        }
    }

    public void ChangeKeyAmount(int keyAmount)
    {
        _keyAmountText.text = keyAmount.ToString();
    }

    public void ChangeLiquidAmount(float value)
    {
        _liquidBarGO.SetActive(true);
        _liquidBar.ChangeValue(value);
    }

    public void RemoveLiquidPeriodicly(Player player)
    {
        _liquidBar.StartRemovingLiquid(player);
    }
    
    public void StopRemovingLiquid()
    {
        _liquidBar.StopRemovingLiquid();
    }
}
