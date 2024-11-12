using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] int _maxHealth = 100;
    int _currentHealth = 0;
    public RainbowColor CurrentRainbowColor {  get; private set; }
    public List<RainbowColor> AvailableRainbowColors { get; private set; }
    IDataService _dataService = new JsonDataService();

    public void ChangeColor(RainbowColor newColor)
    {
        CurrentRainbowColor = newColor;
        GetComponent<SpriteRenderer>().color = CurrentRainbowColor.GetColor();
    }

    private void Awake()
    {
        AvailableRainbowColors = new List<RainbowColor>();
        _currentHealth = _maxHealth;
    }
    private void Start()
    {
        AvailableRainbowColors.Add(RainbowColor.Red);
        AvailableRainbowColors.Add(RainbowColor.Blue);
        CurrentRainbowColor = AvailableRainbowColors[UnityEngine.Random.Range(0, AvailableRainbowColors.Count - 1)];
        GetComponent<SpriteRenderer>().color = CurrentRainbowColor.GetColor();
        UIManager.Instance.ChangeCurrentHealth(_currentHealth);
        UIManager.Instance.ChangeMaxHealth(_maxHealth);
    }

    private void Update()
    {
        // TEMP for testing
        if(Input.GetKeyDown(KeyCode.F1))
        {
            _dataService.SaveData("/player-stats.json", ToPlayerData(), false);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {        
            FromPlayerData(_dataService.LoadData<PlayerData>("/player-stats.json", false));
            Debug.Log(_maxHealth);
        }
    }
    public void Damage(int dmgAmount)
    {
        //int damageAfterElement = (int)CurrentElement.CalculateDamageFrom(element, dmgAmount);
        //_maxHealth -= damageAfterElement;
        _maxHealth = _maxHealth - dmgAmount;
        UIManager.Instance.ChangeCurrentHealth(_maxHealth);

        if (_maxHealth <= 0)
            Destroy(gameObject);
    }

    public void CheckIfHitIsAvailable(BulletPro.Bullet bullet, Vector3 position)
    {
        if(bullet.GetComponent<SpriteRenderer>().color != CurrentRainbowColor.GetColor())
        {
            int damageAmount = bullet.moduleParameters.GetInt("Damage");
            Damage(damageAmount);
            bullet.Die();
        }
    }

    public void SetPosition(Transform newTransform)
    {
        Debug.Log("SetPosition");
        gameObject.transform.position = newTransform.position;
    }

    public PlayerData ToPlayerData()
    {
        return new PlayerData
        {
            hp = _maxHealth,
        };
    }

    public void FromPlayerData(PlayerData playerData)
    {
        _maxHealth = playerData.hp;
    }
}
