using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] int _maxHealth = 100;
    private Color _currentColor = Color.blue;
    private Color _redColor = new Color32(232, 20, 22, 255);
    private Color _blueColor = new Color32(72, 125, 231, 255);
    private int _currentHealth = 0;
    private IDataService _dataService = new JsonDataService();
    

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }
    private void Start()
    {
        UIManager.Instance.ChangeCurrentHealth(_currentHealth);
        UIManager.Instance.ChangeMaxHealth(_maxHealth);
    }

    private void OnEnable()
    {
        PlayerController.ChangeColor += OnChangeColor;
    }

    private void OnDisable()
    {
        PlayerController.ChangeColor -= OnChangeColor;
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
        _maxHealth = _maxHealth - dmgAmount;
        UIManager.Instance.ChangeCurrentHealth(_maxHealth);

        if (_maxHealth <= 0)
            Destroy(gameObject);
    }

    public void CheckIfHitIsAvailable(BulletPro.Bullet bullet, Vector3 position)
    {
        //TODO Add color condition
        if(bullet.GetComponent<SpriteRenderer>().color != _currentColor)
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

    private void OnChangeColor()
    {
        if(_currentColor == _redColor)
        {
            _currentColor = _blueColor;
        }
        else
        {
            _currentColor = _redColor;
        }
        GetComponent<SpriteRenderer>().color = _currentColor;
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
