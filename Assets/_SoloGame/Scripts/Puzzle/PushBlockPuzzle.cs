using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PushBlockPuzzle : MonoBehaviour, IUniqueIdentifier, IState
{
    [SerializeField] private Tilemap _background;
    [SerializeField] private Tilemap _blocked;
    [SerializeField] private Transform _startCell;
    [SerializeField] private Transform _endCell;

    [SerializeField] private string _uniqueID;
    public string UniqueID { get => _uniqueID; set => _uniqueID = value; }
    private PathChecker _pathChecker;
    private PushBlock[] _pushBlocks;
    private List<Vector3> _originalPositions = new List<Vector3>();
    private List<Vector3> _completedPositions = new List<Vector3>();
    private bool _isCompleted = false;

    private void Awake()
    {
        _pathChecker = new PathChecker(_blocked, _background);
        _pushBlocks = GetComponentsInChildren<PushBlock>();
        foreach(PushBlock block in  _pushBlocks)
        {
            Debug.Log("Set og pos");
            _originalPositions.Add(block.transform.position);
        }
    }

    public void SetPuzzle()
    {
        if (GameManager.Instance.IsPuzzleCleared(_uniqueID))
        {
            Debug.Log("<color=green> Is completed");
            List<Vector3> completedPositions = GameManager.Instance.GetCompletedBlockPositions(_uniqueID);
            int index = 0;
            foreach(PushBlock block in _pushBlocks)
            {
                block.gameObject.transform.position = completedPositions[index];
                index++;
            }
        }
        else
        {
            Debug.Log("<color=red> Not completed");
        }
    }

    public void OnPushBlockEndingMovement()
    {
        Vector3Int start = _blocked.WorldToCell(_startCell.position);
        Vector3Int end = _blocked.WorldToCell(_endCell.position);
        HashSet<Vector3Int> blockedCells = GetBlockedCells();

        if(_pathChecker.CheckPath(start, end, blockedCells))
        {
            foreach(PushBlock block in _pushBlocks)
            {
                _completedPositions.Add(block.transform.position);
            }

            _isCompleted = true;
            Debug.Log("Puzzle is completed");
            FloorStateManager.Instance.SaveFloor();
            //GameManager.Instance.MarkPuzzleCleared(_uniqueID, _completedPositions);
        }
    }

    private HashSet<Vector3Int> GetBlockedCells()
    {
        HashSet<Vector3Int> blockedCells = new HashSet<Vector3Int>();

        foreach (var pushBlock in _pushBlocks)
        {
            blockedCells.Add(_blocked.WorldToCell(pushBlock.transform.position));
        }

        return blockedCells;
    }

    public void LoadState(GameState state)
    {

    }

    public void SaveState(GameState state)
    {
        if(_isCompleted)
        {
            if(!state.BoxPuzzle.ContainsKey(_uniqueID))
                state.BoxPuzzle.Add(_uniqueID, _completedPositions);
        }
    }

    public void ResetState()
    {
        if(!_isCompleted)
        {
            for(int i = 0;  i < _pushBlocks.Length; i++)
            {
                Debug.Log(_originalPositions[i]);
                _pushBlocks[i].ResetPosition(_originalPositions[i]);
            }
        }
    }
}
