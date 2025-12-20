using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FloorStateManager : MonoBehaviour
{
    public static FloorStateManager Instance { get; private set; }

    private List<IState> _stateObjects = new();
    public GameState CurrentState { get; private set; }

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
            if(obj is PushBlockPuzzle)
            {
                Debug.Log("Reset puzzle");
                obj.ResetState();
            }
        }
    }
}
