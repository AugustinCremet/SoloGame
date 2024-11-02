using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] int hp = 100;
    [SerializeField] ElementType elementType;
    public Element currentElement;

    private void Start()
    {
        currentElement = ElementManager.instance.GetElementByType(elementType);
        //Temp for prototype visual
        switch (elementType)
        {
            case ElementType.Fire:
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case ElementType.Grass:
                gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case ElementType.Water:
                gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        // TEMP for testing
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentElement = ElementManager.instance.GetElementByType(ElementType.Fire);
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentElement = ElementManager.instance.GetElementByType(ElementType.Grass);
            GetComponent<SpriteRenderer>().color = Color.green;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentElement = ElementManager.instance.GetElementByType(ElementType.Water);
            GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }
    public void Damage(Element element, int dmgAmount)
    {
        int damageAfterElement = (int)currentElement.CalculateDamageFrom(element, dmgAmount);
        Debug.Log($"{dmgAmount} is now {damageAfterElement}");
        hp -= damageAfterElement;

        if (hp <= 0)
            Destroy(gameObject);
    }
}
