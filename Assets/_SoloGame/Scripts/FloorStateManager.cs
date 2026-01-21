using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FloorStateManager : MonoBehaviour
{
    public static FloorStateManager Instance { get; private set; }

    private List<IState> _stateObjects = new();
    public GameState CurrentState { get; private set; }

    [SerializeField] private GameObject[] _floors;
    private bool _isTransitioning = false;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        CurrentState = new GameState();
        _stateObjects = GetComponentsInChildren<IState>().ToList();
    }

    public void LoadFloor(GameState state)
    {
        foreach(var obj in _stateObjects)
        {
            obj.LoadState(state);
        }
    }

    public void SaveFloor()
    {
        foreach(var obj in _stateObjects)
        {
            obj.SaveState(CurrentState);
        }
    }

    public void ResetUncompletedPuzzles()
    {
        foreach(var obj in _stateObjects)
        {
            if(obj is PushBlockPuzzlePathChecker)
            {
                Debug.Log("Reset puzzle");
                obj.ResetState();
            }
        }
    }

    public void ChangeFloor(int floorIndex)
    {
        if(_isTransitioning)
        {
            return;
        }
        StartCoroutine(FloorRoutine(floorIndex));
    }

    private IEnumerator FloorRoutine(int floorIndex)
    {
        _isTransitioning = true;

        var sceneFade = FindAnyObjectByType<SceneFade>();
        yield return sceneFade.FadeOut();

        for(int i = 0; i < _floors.Length; i++)
        {
            _floors[i].SetActive(i == floorIndex);
        }

        yield return null;
        yield return sceneFade.FadeIn();

        _isTransitioning = false;
    }
}
