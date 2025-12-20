using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum LevelType
{
    None,
    MainMenu,
    Gameplay,
    Puzzle
}
public class MySceneManager : MonoBehaviour
{
    public static MySceneManager Instance;
    [SerializeField] Image _transitionImage;
    [SerializeField] float _transitionDuration = 2f;
    private string _essentialScene = "MainGameplayScene";
    private LevelType _type;
    private PlayerController _playerController;
    private bool _isLoading = false;
    private Action _pendingPostAction;

    public string PreviousScene;
    public string CurrentScene { get; private set; }
    public string NextScene;
    public static Action OnChangeScene;

    private Checkpoint _checkpoint;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
         _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void ChangeCurrentScene(string sceneName)
    {
        var dir = _playerController.gameObject.GetComponent<Rigidbody2D>().linearVelocity.normalized;
        SetCheckpoint(_playerController.transform.position + (Vector3)dir);
        CurrentScene = sceneName;
        OnChangeScene?.Invoke();
    }

    public void SetCheckpoint(Vector3 position)
    {
        _checkpoint = new Checkpoint(position);
    }

    public Vector3 GetCheckpointPosition()
    {
        return _checkpoint.Position;
    }

    public void LoadNewLevel(string newScene, LevelType type, Action postAction = null, bool isPuzzleCompleted = false)
    {
        if(_isLoading) return;

        _isLoading = true;
        if(_pendingPostAction == null)
        {
            _pendingPostAction = postAction;
        }

        if(_playerController == null)
        {
            _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        _playerController.SwitchActionMap(InputMode.Loading);
        PreviousScene = CurrentScene;
        NextScene = newScene;
        Debug.Log(newScene);
        _type = type;


        StartCoroutine(StartFadeIn());
    }

    private IEnumerator StartFadeIn()
    {
        float elapsedTime = 0f;

        while(elapsedTime < _transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(0f, 1f, elapsedTime / _transitionDuration);
            _transitionImage.color = new Color(_transitionImage.color.r, _transitionImage.color.g, _transitionImage.color.b, newAlpha);

            yield return null;
        }

        MakeLevelChanges();
    }

    private void MakeLevelChanges()
    {
        UnloadScenes();
        switch(_type)
        {
            case LevelType.MainMenu:
                break;
            case LevelType.Gameplay:
                StartCoroutine(LoadGameplayLevel());
                break;
            case LevelType.Puzzle:
                StartCoroutine(LoadPuzzleLevel());
                break;
            default:
                break;
        }
    }

    private void UnloadScenes()
    {
        List<Scene> loadedScene = new List<Scene>();

        for(int i = 0; i < SceneManager.sceneCount; i++)
        {
            loadedScene.Add(SceneManager.GetSceneAt(i));
        }

        foreach(Scene scene in loadedScene)
        {
            if(scene.name != _essentialScene)
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }
    }

    private IEnumerator LoadGameplayLevel()
    {
        //if(SceneManager.GetSceneByName(_essentialScene) == null)
        //{
        //    AsyncOperation asyncEssential = SceneManager.LoadSceneAsync(_essentialScene, LoadSceneMode.Additive);

        //    while (!asyncEssential.isDone)
        //    {
        //        yield return null;
        //    }
        //}

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(NextScene, LoadSceneMode.Additive);
        _playerController.gameObject.SetActive(true);

        while(!asyncOperation.isDone)
        {
            yield return null;
        }

        _pendingPostAction?.Invoke();
        _pendingPostAction = null;
        StartCoroutine(StartFadeOut(InputMode.Gameplay));
    }

    private IEnumerator LoadPuzzleLevel()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(NextScene, LoadSceneMode.Additive);
        _playerController.gameObject.SetActive(false);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        
        StartCoroutine(StartFadeOut(InputMode.Hamiltonian));
    }

    private IEnumerator StartFadeOut(InputMode mode)
    {
        float elapsedTime = 0f;

        while (elapsedTime < _transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(1f, 0f, elapsedTime / _transitionDuration);
            _transitionImage.color = new Color(_transitionImage.color.r, _transitionImage.color.g, _transitionImage.color.b, newAlpha);

            yield return null;
        }

        EnableControls(mode);
    }

    private void EnableControls(InputMode mode)
    {
        _playerController.SwitchActionMap(mode);
        _isLoading = false;
    }
}
