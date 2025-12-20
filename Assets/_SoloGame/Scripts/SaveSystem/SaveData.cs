using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public LocationData LocationData;
    public PlayerData PlayerData;
    public EnemyData EnemyData;
}

[System.Serializable]
public class LocationData
{
    public string WorldName;
    public string AreaName;
}

[System.Serializable]
public class PlayerData
{
    public int hp;
}

[System.Serializable]
public class EnemyData
{
    public List<string> DeadEnemiesID;
}

[System.Serializable]
public class GameState
{
    public Dictionary<string, List<Vector3>> BoxPuzzle = new();
}
