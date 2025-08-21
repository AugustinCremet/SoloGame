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
    public Tile[] TilesUsed;

    public TileHistory(Vector3Int tilePos, CardinalPoint cardinalPoint, Tile[] tilesUsed)
    {
        TilePos = tilePos;
        CardinalPoint = cardinalPoint;
        TilesUsed = tilesUsed;
    }
}
public class HamiltonianLogic : MonoBehaviour
{
    public static HamiltonianLogic Instance { get; private set; }

    [SerializeField] Tilemap _pathTilemap;
    [SerializeField] Tilemap _endTilemap;

    [SerializeField] TileBase _pathTile;

    [SerializeField] Tile[] _nDeadEnd;
    [SerializeField] Tile[] _sDeadEnd;
    [SerializeField] Tile[] _wDeadEnd;
    [SerializeField] Tile[] _eDeadEnd;
    [SerializeField] Tile[] _fromSToW;
    [SerializeField] Tile[] _fromSToN;
    [SerializeField] Tile[] _fromSToE;
    [SerializeField] Tile[] _fromNToW;
    [SerializeField] Tile[] _fromNToE;
    [SerializeField] Tile[] _fromWToE;

    private CardinalPoint _lastCardinalPoint;
    private Dictionary<(CardinalPoint from, CardinalPoint to), Tile[]> _pathLookup;
    private HashSet<Vector3Int> _visitedTiles = new HashSet<Vector3Int>();
    private Stack<TileHistory> _tilesHistory = new Stack<TileHistory>();
    private int _totalTileCount;
    private float _undoDelay = 0.1f;
    private int _currentFrame = 0;


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

        _pathLookup = new Dictionary<(CardinalPoint from, CardinalPoint to), Tile[]>
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
        _lastCardinalPoint = cardinalPoint;

        Tile[] tiles = DirectionToDeadendTileBase(cardinalPoint);
        Tile tile = tiles[_currentFrame];
        _pathTilemap.SetTile(gridPosition, tile);
        _pathTilemap.RefreshTile(gridPosition);

        _tilesHistory.Push(new TileHistory(gridPosition, cardinalPoint, tiles));
        _visitedTiles.Add(gridPosition);
    }
    private void SetOldTile(Vector3Int gridPosition, CardinalPoint cardinalPoint)
    {
        if(_pathLookup.TryGetValue((_lastCardinalPoint, cardinalPoint), out Tile[] tiles))
        {
            _tilesHistory.Pop();
            _tilesHistory.Push(new TileHistory(gridPosition, _lastCardinalPoint, tiles));
            Tile tile = tiles[_currentFrame];
            _pathTilemap.SetTile(gridPosition, tile);
            _pathTilemap.RefreshTile(gridPosition);
        }
    }
    public void HandleMovementChanges(Vector3Int oldGridPosition, Vector3Int newGridPosition, Vector3 direction)
    {
        AdjustCurrentFrame();
        CardinalPoint cardinalPoint = Vector3ToCardinalPoint(direction);
        SetOldTile(oldGridPosition, cardinalPoint);
        SetCurrentTile(newGridPosition, cardinalPoint);
        AdjustCurrentTiles();

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
        while (_tilesHistory.Count > 1)
        {
            var currentPos = _tilesHistory.Peek();
            _pathTilemap.SetTile(currentPos.TilePos, _pathTile);
            _tilesHistory.Pop();

            var lastPos = _tilesHistory.Peek();
            Tile[] tiles = DirectionToDeadendTileBase(lastPos.CardinalPoint);
            Tile tile = tiles[_currentFrame];
            _pathTilemap.SetTile(lastPos.TilePos, tile);
            _lastCardinalPoint = lastPos.CardinalPoint;

            if(_tilesHistory.Count == 1)
            {
                _visitedTiles.Add(lastPos.TilePos);
            }

            yield return new WaitForSeconds(_undoDelay);
        }
    }

    private void AdjustCurrentFrame()
    {
        if(_currentFrame == 3)
        {
            _currentFrame = 0;
        }
        else
        {
            _currentFrame++;
        }
    }

    private void AdjustCurrentTiles()
    {
        foreach(var tileHistory in _tilesHistory)
        {
            var pos = tileHistory.TilePos;
            var tile = tileHistory.TilesUsed[_currentFrame];

            _pathTilemap.SetTile(pos, tile);
            _pathTilemap.RefreshTile(pos);
        }
        
    }
    private Tile[] DirectionToDeadendTileBase(CardinalPoint direction)
    {
        switch (direction)
        {
            case CardinalPoint.North:
                return _nDeadEnd;
            case CardinalPoint.South:
                return _sDeadEnd;
            case CardinalPoint.East:
                return _eDeadEnd;
            case CardinalPoint.West:
                return _wDeadEnd;
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
