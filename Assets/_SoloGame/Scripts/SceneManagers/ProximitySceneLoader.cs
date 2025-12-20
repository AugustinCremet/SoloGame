using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class ProximitySceneLoader : MonoBehaviour
{
    [SerializeField] List<SceneField> _connectedScenes = new List<SceneField>();
    private string _essentialScene = "MainGameplayScene";
    private string _sceneName;
    [SerializeField] float _cameraSize = 7f;
    public bool IsLoaded {  get; private set; }

    private void Awake()
    {
        _sceneName = gameObject.scene.name;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var newBoundary = gameObject.GetComponentInChildren<PolygonCollider2D>();
            FindFirstObjectByType<CameraConfinerSwitcher>().ChangeBoundary(newBoundary, _cameraSize);
            GameManager.Instance.SetCurrentScene(this);
            LoadScene();
            UnloadScene();
            MySceneManager.Instance.ChangeCurrentScene(_sceneName);
            UpdateAI(UnityEngine.SceneManagement.SceneManager.GetSceneByName(gameObject.name));
            RemoveFG();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
    }

    public void FirstLoad(GameObject essential)
    {
        StartCoroutine(FirstTimeLoad(essential));
    }

    public IEnumerator FirstTimeLoad(GameObject essential)
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);

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

    private void RemoveFG()
    {
        foreach(var scene in _connectedScenes)
        {
            GameObject[] roots = SceneManager.GetSceneByName(scene.SceneName).GetRootGameObjects();

            foreach(var root in roots)
            {
                var fg = root.transform.Find("FG");
                if(fg != null)
                {
                    fg.gameObject.SetActive(false);
                    break;
                }
            }
        }

        GameObject[] roots2 = SceneManager.GetSceneByName(_sceneName).GetRootGameObjects();

        foreach (var root in roots2)
        {
            var fg = root.transform.Find("FG");
            if (fg != null)
            {
                fg.gameObject.SetActive(true);
                break;
            }
        }
    }

    private void LoadScene()
    {
        for(int i = 0; i < _connectedScenes.Count; i++)
        {
            bool isSceneLoaded = false;
            
            for(int j = 0; j < UnityEngine.SceneManagement.SceneManager.sceneCount; j++)
            {
                Scene loadedScene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(j);
                if(loadedScene.name == _connectedScenes[i].SceneName)
                {
                    isSceneLoaded = true;
                    break;
                }
            }

            if(!isSceneLoaded)
            {
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_connectedScenes[i], LoadSceneMode.Additive);
            }
        }
    }

    private void UnloadScene()
    {
        List<Scene> loadedScene = new List<Scene>();

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            loadedScene.Add(SceneManager.GetSceneAt(i));
        }

        foreach (Scene scene in loadedScene)
        {
            bool shouldUnload = true;
            foreach(SceneField  connectedScene in _connectedScenes)
            {
                if(scene.name == connectedScene.SceneName)
                {
                    shouldUnload = false;
                    break;
                }
            }


            if (shouldUnload &&
                scene.name != _essentialScene &&
                scene.name != gameObject.scene.name)
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }

        //for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
        //{
        //    bool shouldSceneUnload = true;
        //    Scene loadedScene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);

        //    for (int j = 0; j < UnityEngine.SceneManagement.SceneManager.sceneCount;  j++)
        //    {
        //        if(loadedScene.name == UnityEngine.SceneManagement.SceneManager.GetSceneAt(j).name || 
        //           loadedScene.name == _sceneName ||
        //           loadedScene.name == _essentialScene)
        //        {
        //            shouldSceneUnload = false;
        //        }
        //    }

        //    if(shouldSceneUnload)
        //    {
        //        Debug.Log("Unload");
        //        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(loadedScene);
        //    }
        //}
    }

    public void UpdateAI(Scene currentPlayerScene)
    {
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCount;

        for (int i = 0; i < sceneCount; i++)
        {
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);

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
