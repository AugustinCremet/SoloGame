using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SceneDetails : MonoBehaviour
{
    [SerializeField] List<SceneDetails> connectedScenes = new List<SceneDetails>();
    public bool IsLoaded {  get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            var newBoundary = gameObject.GetComponentInChildren<PolygonCollider2D>();
            FindFirstObjectByType<CameraConfinerSwitcher>().ChangeBoundary(newBoundary);

            LoadScene();
            GameManager.Instance.SetCurrentScene(this);

            foreach (SceneDetails scene in connectedScenes)
            {
                scene.LoadScene();
            }

            if(GameManager.Instance.PreviousScene != null)
            {
                var previouslyLoadedScenes = GameManager.Instance.PreviousScene.connectedScenes;
                foreach (SceneDetails scene in previouslyLoadedScenes)
                {
                    if(!connectedScenes.Contains(scene) && scene != this)
                    {
                        scene.UnloadScene();
                    }
                }
            }
            UpdateAI(SceneManager.GetSceneByName(gameObject.name));
        }
    }

    private void LoadScene()
    {
        if (!IsLoaded)
        {
            SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);        
            IsLoaded = true;
        }
    }

    private void UnloadScene()
    {
        if(IsLoaded)
        {
            SceneManager.UnloadSceneAsync(gameObject.name);
            IsLoaded = false;
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
