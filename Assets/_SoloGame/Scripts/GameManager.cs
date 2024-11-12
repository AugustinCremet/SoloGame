using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
