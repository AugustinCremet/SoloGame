using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] int _maxHealth = 100;
    int _currentHealth = 0;
    IDataService _dataService = new JsonDataService();

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }
    private void Start()
    {
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
        _maxHealth = _maxHealth - dmgAmount;
        UIManager.Instance.ChangeCurrentHealth(_maxHealth);

        if (_maxHealth <= 0)
            Destroy(gameObject);
    }

    public void CheckIfHitIsAvailable(BulletPro.Bullet bullet, Vector3 position)
    {
        //TODO Add color condition
        //if(bullet.GetComponent<SpriteRenderer>().color !=)
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
