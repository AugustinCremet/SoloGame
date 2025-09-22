using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ProximitySceneLoader : MonoBehaviour
{
    [SerializeField] List<SceneField> _connectedScenes = new List<SceneField>();
    private string _sceneName;
    [SerializeField] float _cameraSize = 7f;
    public bool IsLoaded {  get; private set; }

    private void Awake()
    {
        _sceneName = gameObject.scene.name;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("EnterLoading");
        if (collision.tag == "Player")
        {
            Debug.Log("It is player");
            //StartCoroutine(FirstTimeLoad());
            var newBoundary = gameObject.GetComponent<PolygonCollider2D>();
            FindFirstObjectByType<CameraConfinerSwitcher>().ChangeBoundary(newBoundary, _cameraSize);
            GameManager.Instance.SetCurrentScene(this);
            LoadScene();
            UnloadScene();

            //if (GameManager.Instance.PreviousScene != null)
            //{
            //    var previouslyLoadedScenes = GameManager.Instance.PreviousScene._connectedScenes;
            //    foreach (SceneDetails scene in previouslyLoadedScenes)
            //    {
            //        if (!_connectedScenes.Contains(scene) && scene != this)
            //        {
            //            scene.UnloadScene();
            //        }
            //    }
            //}
            UpdateAI(SceneManager.GetSceneByName(gameObject.name));
        }
    }

    public void FirstLoad(GameObject essential)
    {
        StartCoroutine(FirstTimeLoad(essential));
    }

    public IEnumerator FirstTimeLoad(GameObject essential)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);

        while(!asyncLoad.isDone)
        {
            yield return null;
        }

        IsLoaded = true;
        essential.SetActive(true);
        GameObject savePoint = GameObject.FindGameObjectWithTag("SavePoint");
        //FindAnyObjectByType<SceneTransition>().ResetTransition();
        SceneTransition sceneTransition = FindAnyObjectByType<SceneTransition>();
        FindFirstObjectByType<Player>().SetPosition(savePoint.transform);
        var newBoundary = gameObject.GetComponentInChildren<PolygonCollider2D>();
        FindFirstObjectByType<CameraConfinerSwitcher>().ChangeBoundary(newBoundary, _cameraSize);
        yield return sceneTransition.FadeOut(true);
    }

    private void LoadScene()
    {
        for(int i = 0; i < _connectedScenes.Count; i++)
        {
            bool isSceneLoaded = false;
            for(int j = 0; j < SceneManager.sceneCount; j++)
            {
                Scene loadedScene = SceneManager.GetSceneAt(j);
                if(loadedScene.name == _connectedScenes[i].SceneName)
                {
                    isSceneLoaded = true;
                    break;
                }
            }

            if(!isSceneLoaded)
            {
                SceneManager.LoadSceneAsync(_connectedScenes[i], LoadSceneMode.Additive);
            }
        }
    }

    private void UnloadScene()
    {
        for(int i = 0; i < SceneManager.sceneCount; i++)
        {
            bool shouldSceneUnload = true;
            Scene loadedScene = SceneManager.GetSceneAt(i);

            for (int j = 0; j < _connectedScenes.Count;  j++)
            {
                if(loadedScene.name == _connectedScenes[j] || loadedScene.name == _sceneName)
                {
                    shouldSceneUnload = false;
                }
            }

            if(shouldSceneUnload)
            {
                SceneManager.UnloadSceneAsync(loadedScene);
            }
        }
    }

    public void UpdateAI(Scene currentPlayerScene)
    {
        int sceneCount = SceneManager.sceneCount;

        for (int i = 0; i < sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (!scene.isLoaded)
                continue;

            bool isCurrentScene = scene == currentPlayerScene;

            // Loop through root GameObjects in this scene
            foreach (GameObject rootObj in scene.GetRootGameObjects())
            {
                Enemy[] agents = rootObj.GetComponentsInChildren<Enemy>(true);

                foreach (Enemy enemy in agents)
                {
                    enemy.SetAI(isCurrentScene);
                }
            }
        }
    }
}
