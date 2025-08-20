using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum CardinalPoint
{
    North,
    South,
    East,
    West,
}

public struct TileHistory
{
    public Vector3Int TilePos;
    public CardinalPoint CardinalPoint;

    public TileHistory(Vector3Int tilePos, CardinalPoint cardinalPoint)
    {
        TilePos = tilePos;
        CardinalPoint = cardinalPoint;
    }
}
public class HamiltonianLogic : MonoBehaviour
{
    public static HamiltonianLogic Instance { get; private set; }

    [SerializeField] Tilemap _pathTilemap;
    [SerializeField] Tilemap _endTilemap;

    [SerializeField] TileBase _pathTile;

    [SerializeField] TileBase _nDeadend;
    [SerializeField] TileBase _sDeadend;
    [SerializeField] TileBase _wDeadend;
    [SerializeField] TileBase _eDeadend;
    [SerializeField] TileBase _fromSToW;
    [SerializeField] TileBase _fromSToN;
    [SerializeField] TileBase _fromSToE;
    [SerializeField] TileBase _fromNToW;
    [SerializeField] TileBase _fromNToE;
    [SerializeField] TileBase _fromWToE;

    private CardinalPoint _lastCardinalPoint;
    private Dictionary<(CardinalPoint from, CardinalPoint to), TileBase> _pathLookup;
    private HashSet<Vector3Int> _visitedTiles = new HashSet<Vector3Int>();
    private Stack<TileHistory> _movementHistory = new Stack<TileHistory>();
    private int _totalTileCount;
    private float _undoDelay = 0.1f;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        _pathLookup = new Dictionary<(CardinalPoint from, CardinalPoint to), TileBase>
        {
            {(CardinalPoint.North, CardinalPoint.North), _fromSToN },
            {(CardinalPoint.North, CardinalPoint.East), _fromSToE },
            {(CardinalPoint.North, CardinalPoint.West), _fromSToW },
            {(CardinalPoint.East, CardinalPoint.East), _fromWToE },
            {(CardinalPoint.East, CardinalPoint.South), _fromSToW },
            {(CardinalPoint.East, CardinalPoint.North), _fromNToW },
            {(CardinalPoint.South, CardinalPoint.South), _fromSToN },
            {(CardinalPoint.South, CardinalPoint.West), _fromNToW},
            {(CardinalPoint.South, CardinalPoint.East), _fromNToE},
            {(CardinalPoint.West, CardinalPoint.West), _fromWToE},
            {(CardinalPoint.West, CardinalPoint.North), _fromNToE },
            {(CardinalPoint.West, CardinalPoint.South), _fromSToE },
        };
    }

    private void Start()
    {
        foreach(var pos in _pathTilemap.cellBounds.allPositionsWithin)
        {
            if(_pathTilemap.HasTile(pos))
            {
                _totalTileCount++;
            }
        }

        int tileAmount = 0;
        foreach(var pos in _endTilemap.cellBounds.allPositionsWithin)
        {
            if(_endTilemap.HasTile(pos))
            {
                tileAmount++;
            }
        }
        if(tileAmount > 1)
        {
            Debug.LogWarning("<color=red>More than one end tile");
        }
    }

    public bool WasTileVisited(Vector3Int gridPosition)
    {
        return _visitedTiles.Contains(gridPosition);
    }

    public void SetCurrentTile(Vector3Int gridPosition, CardinalPoint cardinalPoint)
    {
        _movementHistory.Push(new TileHistory(gridPosition, cardinalPoint));
        _visitedTiles.Add(gridPosition);
        _lastCardinalPoint = cardinalPoint;
        TileBase tileBase = DirectionToDeadendTileBase(cardinalPoint);
        _pathTilemap.SetTile(gridPosition, tileBase);
        _pathTilemap.RefreshTile(gridPosition);
    }
    private void SetOldTile(Vector3Int gridPosition, CardinalPoint cardinalPoint)
    {
        if(_pathLookup.TryGetValue((_lastCardinalPoint, cardinalPoint), out TileBase tileBase))
        {
            _pathTilemap.SetTile(gridPosition, tileBase);
            _pathTilemap.RefreshTile(gridPosition);
        }
    }
    public void HandleMovementChanges(Vector3Int oldGridPosition, Vector3Int newGridPosition, Vector3 direction)
    {
        CardinalPoint cardinalPoint = Vector3ToCardinalPoint(direction);
        SetOldTile(oldGridPosition, cardinalPoint);
        SetCurrentTile(newGridPosition, cardinalPoint);

        if(_endTilemap.HasTile(newGridPosition))
        {
            HandleEnd();
        }
    }

    private void HandleEnd()
    {
        if (_visitedTiles.Count == _totalTileCount)
        {
            Debug.Log("<color=green>Win");
        }
        else
        {
            Debug.Log("<color=red>Fail");
        }
    }

    public void Restart()
    {
        _visitedTiles.Clear();
        StartCoroutine(UndoPathCR());
    }

    private IEnumerator UndoPathCR()
    {
        while (_movementHistory.Count > 1)
        {
            var currentPos = _movementHistory.Peek();
            _pathTilemap.SetTile(currentPos.TilePos, _pathTile);
            _movementHistory.Pop();

            var lastPos = _movementHistory.Peek();
            TileBase deadendTile = DirectionToDeadendTileBase(lastPos.CardinalPoint);
            _pathTilemap.SetTile(lastPos.TilePos, deadendTile);
            _lastCardinalPoint = lastPos.CardinalPoint;

            if(_movementHistory.Count == 1)
            {
                _visitedTiles.Add(lastPos.TilePos);
            }

            yield return new WaitForSeconds(_undoDelay);
        }
    }

    private TileBase DirectionToDeadendTileBase(CardinalPoint direction)
    {
        switch (direction)
        {
            case CardinalPoint.North:
                return _nDeadend;
            case CardinalPoint.South:
                return _sDeadend;
            case CardinalPoint.East:
                return _eDeadend;
            case CardinalPoint.West:
                return _wDeadend;
            default:
                return null;
        }
    }

    private CardinalPoint Vector3ToCardinalPoint(Vector3 direction)
    {
        if (direction == Vector3.up)
            return CardinalPoint.North;
        else if (direction == Vector3.down)
            return CardinalPoint.South;
        else if (direction == Vector3.right)
            return CardinalPoint.East;
        else if (direction == Vector3.left)
            return CardinalPoint.West;

        return CardinalPoint.North;
    }
}
