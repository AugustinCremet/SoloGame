using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    SceneDetails _currentScene;
    public SceneDetails PreviousScene { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
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
        if(SceneManager.GetActiveScene().name != "WorldMap")
            SceneManager.LoadSceneAsync("WorldMap");
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
