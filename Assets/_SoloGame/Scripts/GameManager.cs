using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private SceneDetails _currentScene;
    private string _currentSceneParentName;
    public SceneDetails PreviousScene { get; private set; }
    [SerializeField] GameObject _essentialPrefab;
    private GameObject _essential;

    private string _savePath = "/Game1.json";
    private IDataService _dataService = new JsonDataService();
    public static event Func<SaveData> OnSave;
    public static event Action<SaveData> OnLoad;

    private List<string> _tempClearedWaveIDs = new List<string>();

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
        SaveData fullSaveData = new SaveData
        {
            LocationData = new LocationData
            {
                WorldName = _currentSceneParentName,
                AreaName = _currentScene.gameObject.name
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
        StartCoroutine(LoadingScreen());
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
        areaGameObject.GetComponent<SceneDetails>().FirstLoad(_essential);
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

    public void ResetScenes()
    {
        StartCoroutine(UnloadAllScenesExcept("MainArea"));
    }

    private IEnumerator UnloadAllScenesExcept(string sceneToKeep)
    {
        _essential.SetActive(false);

        List<Scene> scenesToUnload = new List<Scene>();

        // Collect all scenes to unload first
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded && scene.name != sceneToKeep)
            {
                scenesToUnload.Add(scene);
            }
        }

        // Now unload them safely
        foreach (Scene scene in scenesToUnload)
        {
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(scene);
            while (!unloadOp.isDone)
                yield return null;
        }

        Debug.Log("Unload is done");
        LoadGame();
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

    public void SetCurrentScene(SceneDetails scene)
    {
        PreviousScene = _currentScene;
        _currentScene = scene;
        _currentSceneParentName = SceneManager.GetActiveScene().name;
    }

    public void SetSaveSlot(int slot)
    {
        _savePath = $"/Game{slot}.json";
    }

    public SceneDetails GetPreviousScene()
    {
        return PreviousScene;
    }
}
