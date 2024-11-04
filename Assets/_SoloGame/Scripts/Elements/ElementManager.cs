using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementManager : MonoBehaviour
{
    public static ElementManager Instance { get; private set; }

    List<Element> elements;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
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
