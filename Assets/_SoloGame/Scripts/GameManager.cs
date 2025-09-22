using System;
using System.Collections;
using System.Collections.Generic;
using BulletPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private ProximitySceneLoader _currentScene;
    private string _currentSceneParentName;
    public ProximitySceneLoader PreviousScene { get; private set; }
    [SerializeField] GameObject _essentialPrefab;
    [SerializeField] bool _isUniqueScene;
    private GameObject _essential;

    private string _savePath = "/Game1.json";
    private IDataService _dataService = new JsonDataService();
    public static event Func<SaveData> OnSave;
    public static event Action<SaveData> OnLoad;

    private List<string> _tempClearedWaveIDs = new List<string>();
    private Dictionary<string, bool> _tempClearedEnemies = new Dictionary<string, bool>();
    private List<string> _permaClearedEnemies = new List<string>();

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        if (FindAnyObjectByType<EssentialObjects>() == null)
        {
            _essential = Instantiate(_essentialPrefab);
            DontDestroyOnLoad(this);
            //transform.SetParent(essential.transform);
        }

        if(_isUniqueScene)
        {
            _essential.SetActive(true);
        }
    }

    private void Start()
    {
        if(_isUniqueScene)
        {
            var enemies = Resources.FindObjectsOfTypeAll(typeof(Enemy));
            foreach(Enemy enemy in enemies)
            {
                enemy.SetAI(true);
            }
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

    public void MarkEnemyTempDead(string uniqueID, bool canBePerma)
    {
        if(!_tempClearedEnemies.ContainsKey(uniqueID))
        {
            _tempClearedEnemies.Add(uniqueID, canBePerma);
        }
    }

    public bool IsEnemyTempDead(string _uniqueID)
    {
        return _tempClearedEnemies.ContainsKey(_uniqueID);
    }

    private void HandleTempData()
    {
        List<string> keysToRemove = new List<string>();

        foreach(var pair in _tempClearedEnemies)
        {
            if(pair.Value)
            {
                _permaClearedEnemies.Add(pair.Key);
            }
            else
            {
                keysToRemove.Add(pair.Key);
            }
        }
        foreach(var key in keysToRemove)
        {
            _tempClearedEnemies.Remove(key);
        }
    }

    public void MarkWaveCleared(string uniqueID)
    {
        if(!_tempClearedWaveIDs.Contains(uniqueID))
        {
            _tempClearedWaveIDs.Add(uniqueID);
        }
    }

    public bool IsWaveCleared(string uniqueID)
    {
        return _tempClearedWaveIDs.Contains(uniqueID);
    }

    public void SaveGame()
    {
        HandleTempData();

        SaveData fullSaveData = new SaveData
        {
            LocationData = new LocationData
            {
                WorldName = _currentSceneParentName,
                AreaName = _currentScene.gameObject.name
            },
            EnemyData = new EnemyData
            {
                DeadEnemiesID = _permaClearedEnemies
            }
        };


        if (OnSave != null)
        {
            foreach (Func<SaveData> saveFunction in OnSave.GetInvocationList())
            {
                SaveData partialData = saveFunction.Invoke();
                SaveDataMerger.Merge(fullSaveData, partialData);
            }
        }

        _dataService.SaveData(_savePath, fullSaveData, false);
    }

    public void LoadGame()
    {
        DestroyBullets();
        _essential.SetActive(false);
        StartCoroutine(LoadingScreen());
    }

    public void DestroyBullets()
    {
        var bulletEmitters = FindObjectsByType<BulletEmitter>(FindObjectsSortMode.None);
        foreach(var bulletEmitter in bulletEmitters)
        {
            foreach(var bullet in bulletEmitter.bullets)
            {
                bullet.moduleRenderer.Disable();
            }
            bulletEmitter.Kill();
        }
    }

    public IEnumerator LoadingScreen()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LoadingScreen");
        while (!asyncLoad.isDone && asyncLoad != null)
        {
            yield return null;
        }
        StartCoroutine(LoadSceneAsync());
        OnLoad?.Invoke(_dataService.LoadData<SaveData>(_savePath, false));
    }

    private IEnumerator LoadSceneAsync()
    {
        string sceneName = _dataService.LoadData<SaveData>(_savePath, false).LocationData.WorldName;

        AsyncOperation asyncLoad = null;
        asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        while (!asyncLoad.isDone && asyncLoad != null)
        {
            yield return null;
        }

        GameObject areaGameObject = GameObject.Find(_dataService.LoadData<SaveData>(_savePath, false).LocationData.AreaName);
        areaGameObject.GetComponent<ProximitySceneLoader>().FirstLoad(_essential);
    }

    public static bool IsSceneLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == sceneName)
            {
                return scene.isLoaded;
            }
        }
        return false;
    }

    public void StartPuzzle(string sceneName)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>().enabled = false;
        GameObject.FindAnyObjectByType<SceneTransition>().StartTransition(sceneName, true);
    }

    public void EndPuzzle(string sceneName)
    {
        GameObject.FindAnyObjectByType<SceneTransition>().StartTransition(sceneName, false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>().enabled = true;
    }

    public void SetCurrentScene(ProximitySceneLoader scene)
    {
        PreviousScene = _currentScene;
        _currentScene = scene;
        _currentSceneParentName = SceneManager.GetActiveScene().name;
    }

    public void SetSaveSlot(int slot)
    {
        _savePath = $"/Game{slot}.json";
    }

    public ProximitySceneLoader GetPreviousScene()
    {
        return PreviousScene;
    }
}
