using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public PlayerData PlayerData;
    public EnemyData EnemyData;
}

[System.Serializable]
public class PlayerData
{
    public int hp;
}

[System.Serializable]
public class EnemyData
{
    public int hp;
}
public class JsonDataService : IDataService
{
    public bool SaveData<T>(string relativePath, T Data, bool encrypted)
    {
        string path = Application.persistentDataPath + relativePath;

        try
        {
            if (File.Exists(path))
            {
                Debug.Log("Data exists. Deleting old file and writing a new one!");
                File.Delete(path);
            }
            else
            {
                Debug.Log("Creating file for the first time!");
            }
            using FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, JsonConvert.SerializeObject(Data));
            return true;
        }
        catch(Exception e)
        {
            Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
            return false;
        }

    }
    public T LoadData<T>(string relativePath, bool Encrypted)
    {
        string path = Application.persistentDataPath + relativePath;

        if(!File.Exists(path))
        {
            Debug.LogError($"Cannot load file at {path}. File does not exist!");
            throw new FileNotFoundException($"{path} does not exist!");
        }

        try
        {
            T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
            throw e;
        }
    }
}
