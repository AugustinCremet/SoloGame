using UnityEngine;

public class PipeManager : MonoBehaviour
{
    [SerializeField] GameObject[] _floorPipes;
    private void EnableCorrectPipes()
    {
        string currentScene = MySceneManager.Instance.CurrentScene;
        foreach (GameObject floorPipe in _floorPipes)
        {
            if(currentScene.Contains(floorPipe.name))
            {
                floorPipe.SetActive(true);
            }
            else
            {
                floorPipe.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        MySceneManager.OnChangeScene += EnableCorrectPipes;
    }

    private void OnDisable()
    {
        MySceneManager.OnChangeScene -= EnableCorrectPipes;
    }
}
