using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementManager : MonoBehaviour
{
    public static ElementManager instance { get; private set; }

    List<Element> elements;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            LoadAllElements();
            DontDestroyOnLoad(gameObject);
        }
    }

    void LoadAllElements()
    {
        elements = new List<Element>(Resources.LoadAll<Element>("Elements"));
    }

    public Element GetElementByType(ElementType type)
    {
        foreach (Element element in elements)
        {
            if (element.elementType == type)
                return element;
        }

        Debug.LogWarning($"Element type {type} not found.");
        return null;
    }
}
