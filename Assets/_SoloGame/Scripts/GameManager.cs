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

    private Dictionary<string, bool> _tempPuzzleCompleted = new Dictionary<string, bool>();
    private Dictionary<string, List<Vector3>> _tempBlockPositions = new Dictionary<string, List<Vector3>>();
    private Dictionary<string, bool> _tempCollectedCollectable = new Dictionary<string, bool>();

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
            var player = GameObject.FindWithTag("Player");
            player.transform.position = transform.position;
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

    public void SetEssentialGameplayObject(bool isActive)
    {
        _essential.SetActive(isActive);

        // AC_TODO remove
        var player = GameObject.FindWithTag("Player");
        player.transform.position = transform.position;
        // 
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
        Debug.Log("UniqueID = " +  _uniqueID);
        if(_uniqueID == "")
        {
            Debug.LogWarning("This enemy doesn't have an ID, ignore if intentional");
            return false;
        }

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

    public void MarkPuzzleCleared(string uniqueID, List<Vector3> positions = null)
    {
        if(!_tempPuzzleCompleted.ContainsKey(uniqueID))
        {
            _tempPuzzleCompleted[uniqueID] = true;

            if(positions != null)
            {
                _tempBlockPositions[uniqueID] = positions;
            }
        }
    }

    public bool IsPuzzleCleared(string uniqueID)
    {
        return _tempPuzzleCompleted.TryGetValue(uniqueID, out var result);
    }

    public List<Vector3> GetCompletedBlockPositions(string uniqueID)
    {
        if(_tempBlockPositions.TryGetValue(uniqueID, out var result))
        {
            return result;
        }

        return null;
    }

    public void MarkCollectableColledted(string uniqueID)
    {
        if(!_tempCollectedCollectable.ContainsKey(uniqueID))
        {
            _tempCollectedCollectable[uniqueID] = true;
        }
    }

    public bool IsCollectableCollected(string uniqueID)
    {
        if(_tempCollectedCollectable.TryGetValue(uniqueID, out var result))
        {
            return result;
        }

        return false;
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
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("LoadingScreen");
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
        asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        while (!asyncLoad.isDone && asyncLoad != null)
        {
            yield return null;
        }

        GameObject areaGameObject = GameObject.Find(_dataService.LoadData<SaveData>(_savePath, false).LocationData.AreaName);
        areaGameObject.GetComponent<ProximitySceneLoader>().FirstLoad(_essential);
    }

    public void SetCurrentScene(ProximitySceneLoader scene)
    {
        PreviousScene = _currentScene;
        _currentScene = scene;
        _currentSceneParentName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    public void SetSaveSlot(int slot)
    {
        _savePath = $"/Game{slot}.json";
    }
}
