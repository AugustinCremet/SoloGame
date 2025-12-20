using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MapChanger : MonoBehaviour
{
    [SerializeField] SceneField _connectedScene;
    [SerializeField] Transform _spawnLocation;
    private BoxCollider2D _col;

    private void Awake()
    {
        _col = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            MySceneManager.Instance.LoadNewLevel(_connectedScene, LevelType.Gameplay);
            //Debug.Log("Enter");
            //var player = FindFirstObjectByType<Player>();
            //if(!player.JustTeleportedCD.IsReady)
            //{
            //    return;
            //}
            //player.JustTeleportedCD.Use();
            //StartCoroutine(SwitchScene());
        }
    }

    private IEnumerator SwitchScene()
    {
        DontDestroyOnLoad(gameObject);
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("LoadingScreen", LoadSceneMode.Additive);
        while (!asyncLoad.isDone && asyncLoad != null)
        {
            yield return null;
        }
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = null;
        asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_connectedScene, LoadSceneMode.Single);

        while (!asyncLoad.isDone && asyncLoad != null)
        {
            yield return null;
        }

        var player = FindFirstObjectByType<Player>();
        player.SetPosition(_spawnLocation);
        Destroy(gameObject);
    }
}
