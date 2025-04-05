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
            LoadScene();
            GameManager.Instance.SetCurrentScene(this);
            UpdateNavAgents(SceneManager.GetSceneByName(gameObject.name));

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

    public void UpdateNavAgents(Scene currentPlayerScene)
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
                NavMeshAgent[] agents = rootObj.GetComponentsInChildren<NavMeshAgent>(true);

                foreach (NavMeshAgent agent in agents)
                {
                    agent.enabled = isCurrentScene;
                }
            }
        }
    }
}
