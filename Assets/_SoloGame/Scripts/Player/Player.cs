using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] int _maxHealth = 100;
    int _currentHealth = 0;

    [SerializeField] GameObject _rainbowColorsPrefab;
    public RainbowColor CurrentRainbowColor {  get; private set; }
    public List<RainbowColor> AvailableRainbowColors { get; private set; }
    IDataService _dataService = new JsonDataService();

    public void ChangeColor(RainbowColor newColor)
    {
        CurrentRainbowColor = newColor;
        GetComponent<SpriteRenderer>().color = CurrentRainbowColor.GetColor();
    }

    public void ChangeColorV(bool isNextColor)
    {
        int nextIndex = -1;
        if (isNextColor)
        {
            nextIndex = AvailableRainbowColors.IndexOf(CurrentRainbowColor) + 1;
        }
        else
        {
            nextIndex = AvailableRainbowColors.IndexOf(CurrentRainbowColor) - 1;
        }

        if(nextIndex >= AvailableRainbowColors.Count)
        {
            CurrentRainbowColor = AvailableRainbowColors[0];
        }
        else if(nextIndex < 0)
        {
            CurrentRainbowColor = AvailableRainbowColors[AvailableRainbowColors.Count - 1];
        }
        else
        {
            CurrentRainbowColor = AvailableRainbowColors[nextIndex];
        }
        GetComponent<SpriteRenderer>().color = CurrentRainbowColor.GetColor();
    }

    private void OnEnable()
    {
        ColorSwitcherController.onNextColorSwitch += ChangeColorV;
    }

    private void OnDisable()
    {
        ColorSwitcherController.onNextColorSwitch -= ChangeColorV;
    }

    private void Awake()
    {
        AvailableRainbowColors = new List<RainbowColor>();
        _currentHealth = _maxHealth;
    }
    private void Start()
    {
        GameObject rainbowColorsPrefab = Instantiate(_rainbowColorsPrefab, transform.parent.transform);
        AvailableRainbowColors.Add(RainbowColor.Red);
        AvailableRainbowColors.Add(RainbowColor.Orange);
        AvailableRainbowColors.Add(RainbowColor.Yellow);
        AvailableRainbowColors.Add(RainbowColor.Green);
        AvailableRainbowColors.Add(RainbowColor.Blue);
        AvailableRainbowColors.Add(RainbowColor.Indigo);
        AvailableRainbowColors.Add(RainbowColor.Violet);
        rainbowColorsPrefab.GetComponent<RainbowColorsBehavior>().Initiate(AvailableRainbowColors, transform);
        CurrentRainbowColor = RainbowColor.Red;
        GetComponent<SpriteRenderer>().color = CurrentRainbowColor.GetColor();
        UIManager.Instance.ChangeCurrentHealth(_currentHealth);
        UIManager.Instance.ChangeMaxHealth(_maxHealth);
    }

    private void Update()
    {
        // TODO for testing
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
