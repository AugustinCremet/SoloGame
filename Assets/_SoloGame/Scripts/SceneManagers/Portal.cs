using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// AC_TODO Probably need to put real names eventually
public enum DestinationIdentifier { A, B, C, D, E }
public class Portal : MonoBehaviour
{
    [SerializeField] int _sceneToLoad = -1;
    [SerializeField] DestinationIdentifier _destinationPortal;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(SwitchScene());
    }

    IEnumerator SwitchScene()
    {
        DontDestroyOnLoad(gameObject);
        yield return SceneManager.LoadSceneAsync(_sceneToLoad);

        var desinationPortal = FindObjectsOfType<Portal>().First(x => x != this && x._destinationPortal == _destinationPortal);
        var player = FindFirstObjectByType<Player>();
        player.SetPosition(desinationPortal.transform.GetChild(0).transform);
        Destroy(gameObject);
    }
}
