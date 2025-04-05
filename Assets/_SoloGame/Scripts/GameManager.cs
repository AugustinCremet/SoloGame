using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private SceneDetails _currentScene;
    public SceneDetails PreviousScene { get; private set; }
    [SerializeField] GameObject _essentialPrefab;

    private string _savePath = "/Game1.json";
    private IDataService _dataService = new JsonDataService();
    public static event Func<SaveData> OnSave;
    public static event Action<SaveData> OnLoad;

    private void Awake()
    {
        if (FindAnyObjectByType<EssentialObjects>() == null)
        {
            var essential = Instantiate(_essentialPrefab);
            transform.SetParent(essential.transform);
        }

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SaveGame();
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            LoadGame();
        }
    }

    public void SaveGame()
    {
        SaveData fullSaveData = new SaveData();

        if(OnSave != null)
        {
            foreach(Func<SaveData> saveFunction in OnSave.GetInvocationList())
            {
                SaveData partialData = saveFunction.Invoke();
                SaveDataMerger.Merge(fullSaveData, partialData);
            }
        }

        _dataService.SaveData(_savePath, fullSaveData, false);
    }

    public void LoadGame()
    {
        OnLoad?.Invoke(_dataService.LoadData<SaveData>(_savePath, false));
    }

    public void SetCurrentScene(SceneDetails scene)
    {
        PreviousScene = _currentScene;
        _currentScene = scene;
    }

    public SceneDetails GetPreviousScene()
    {
        return PreviousScene;
    }
}
