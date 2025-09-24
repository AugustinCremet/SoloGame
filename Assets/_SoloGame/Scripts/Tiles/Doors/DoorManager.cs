using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public static DoorManager Instance { get; private set; }
    public static Action<string> OnDoorUnlocked;

    private List<string> _unlockedDoors = new List<string>();


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void UnlockDoor(string id)
    {
        if(string.IsNullOrEmpty(id) || _unlockedDoors.Contains(id))
        {
            return;
        }

        _unlockedDoors.Add(id);
        OnDoorUnlocked?.Invoke(id);
    }

    public bool IsUnlocked(string id)
    {
        return _unlockedDoors.Contains(id);
    }
}
