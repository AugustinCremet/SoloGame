using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] int _maxHealth = 100;
    int _currentHealth = 0;
    [SerializeField] RainbowColor RainbowColor;
    //[SerializeField] ElementType _elementType;
    //public Element CurrentElement;

    IDataService _dataService = new JsonDataService();

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }
    private void Start()
    {
        //CurrentElement = ElementManager.Instance.GetElementByType(_elementType);
        GetComponent<SpriteRenderer>().color = RainbowColor.GetColor();
        UIManager.Instance.ChangeCurrentHealth(_currentHealth);
        UIManager.Instance.ChangeMaxHealth(_maxHealth);

        //Temp for prototype visual
        //switch (_elementType)
        //{
        //    case ElementType.Fire:
        //        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        //        break;
        //    case ElementType.Grass:
        //        gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        //        break;
        //    case ElementType.Water:
        //        gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        //        break;
        //    default:
        //        break;
        //}
    }

    private void Update()
    {
        // TEMP for testing
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //CurrentElement = ElementManager.Instance.GetElementByType(ElementType.Fire);
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //CurrentElement = ElementManager.Instance.GetElementByType(ElementType.Grass);
            GetComponent<SpriteRenderer>().color = Color.green;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //CurrentElement = ElementManager.Instance.GetElementByType(ElementType.Water);
            GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else if(Input.GetKeyDown(KeyCode.F1))
        {
            _dataService.SaveData("/player-stats.json", ToPlayerData(), false);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {        
            FromPlayerData(_dataService.LoadData<PlayerData>("/player-stats.json", false));
            Debug.Log(_maxHealth);
        }
    }
    public void Damage(Element element, int dmgAmount)
    {
        //int damageAfterElement = (int)CurrentElement.CalculateDamageFrom(element, dmgAmount);
        //_maxHealth -= damageAfterElement;
        UIManager.Instance.ChangeCurrentHealth(_maxHealth);

        if (_maxHealth <= 0)
            Destroy(gameObject);
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
