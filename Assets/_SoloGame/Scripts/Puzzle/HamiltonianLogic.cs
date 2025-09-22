using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum CardinalPoint
{
    None,
    North,
    South,
    East,
    West,
}

public struct TileHistory
{
    public Vector3Int TilePos;
    public CardinalPoint EnterTileToward;
    public CardinalPoint ExitTileToward;
    public Tile TileUsed;

    public TileHistory(Vector3Int tilePos, CardinalPoint enterTileToward, CardinalPoint exitTileToward, Tile tilesUsed)
    {
        TilePos = tilePos;
        EnterTileToward = enterTileToward;
        ExitTileToward = exitTileToward;
        TileUsed = tilesUsed;
    }
}
public class HamiltonianLogic : MonoBehaviour
{
    public static HamiltonianLogic Instance { get; private set; }

    [SerializeField] Tilemap _pathTilemap;
    [SerializeField] Tilemap _endTilemap;
    [SerializeField] Tilemap _gooTilemap;

    [SerializeField] TileBase _pathTile;

    [SerializeField] Tile _nDeadEnd;
    [SerializeField] Tile _sDeadEnd;
    [SerializeField] Tile _wDeadEnd;
    [SerializeField] Tile _eDeadEnd;
    [SerializeField] Tile _enterSExitWBig;
    [SerializeField] Tile _enterSExitSBig;
    [SerializeField] Tile _enterSExitEBig;
    [SerializeField] Tile _enterNExitEBig;
    [SerializeField] Tile _enterNExitNBig;
    [SerializeField] Tile _enterNExitWBig;
    [SerializeField] Tile _enterWExitNBig;
    [SerializeField] Tile _enterWExitWBig;
    [SerializeField] Tile _enterWExitSBig;
    [SerializeField] Tile _enterEExitSBig;
    [SerializeField] Tile _enterEExitEBig;
    [SerializeField] Tile _enterEExitNBig;
    [SerializeField] Tile _fromSToWSmall;
    [SerializeField] Tile _fromSToNSmall;
    [SerializeField] Tile _fromSToESmall;
    [SerializeField] Tile _fromNToWSmall;
    [SerializeField] Tile _fromNToESmall;
    [SerializeField] Tile _fromWToESmall;

    private CardinalPoint _lastCardinalPoint;
    private Dictionary<(CardinalPoint enterToward, CardinalPoint exitToward), Tile> _pathLookupBig;
    private Dictionary<(CardinalPoint from, CardinalPoint to), Tile> _pathLookupSmall;
    private HashSet<Vector3Int> _visitedTiles = new HashSet<Vector3Int>();
    private List<TileHistory> _tilesHistory = new List<TileHistory>();
    private int _totalTileCount;
    private float _undoDelay = 0.1f;
    private int _currentTile = 0;


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

        _pathLookupBig = new Dictionary<(CardinalPoint enterToward, CardinalPoint exitToward), Tile>
        {
            {(CardinalPoint.North, CardinalPoint.North), _enterNExitNBig },
            {(CardinalPoint.North, CardinalPoint.East),  _enterNExitEBig },
            {(CardinalPoint.North, CardinalPoint.West),  _enterNExitWBig },
            {(CardinalPoint.East,  CardinalPoint.East),  _enterEExitEBig },
            {(CardinalPoint.East,  CardinalPoint.South), _enterEExitSBig },
            {(CardinalPoint.East,  CardinalPoint.North), _enterEExitNBig },
            {(CardinalPoint.South, CardinalPoint.South), _enterSExitSBig },
            {(CardinalPoint.South, CardinalPoint.West),  _enterSExitWBig },
            {(CardinalPoint.South, CardinalPoint.East),  _enterSExitEBig },
            {(CardinalPoint.West,  CardinalPoint.West),  _enterWExitWBig },
            {(CardinalPoint.West,  CardinalPoint.North), _enterWExitNBig },
            {(CardinalPoint.West,  CardinalPoint.South), _enterWExitSBig },
        };

        _pathLookupSmall = new Dictionary<(CardinalPoint enterToward, CardinalPoint exitToward), Tile>
        {
            {(CardinalPoint.North, CardinalPoint.North), _fromSToNSmall },
            {(CardinalPoint.North, CardinalPoint.East),  _fromSToESmall },
            {(CardinalPoint.North, CardinalPoint.West),  _fromSToWSmall },
            {(CardinalPoint.East,  CardinalPoint.East),  _fromWToESmall },
            {(CardinalPoint.East,  CardinalPoint.South), _fromSToWSmall },
            {(CardinalPoint.East,  CardinalPoint.North), _fromNToWSmall },
            {(CardinalPoint.South, CardinalPoint.South), _fromSToNSmall },
            {(CardinalPoint.South, CardinalPoint.West),  _fromNToWSmall },
            {(CardinalPoint.South, CardinalPoint.East),  _fromNToESmall },
            {(CardinalPoint.West,  CardinalPoint.West),  _fromWToESmall },
            {(CardinalPoint.West,  CardinalPoint.North), _fromNToESmall },
            {(CardinalPoint.West,  CardinalPoint.South), _fromSToESmall },
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

    public void SetCurrentTile(Vector3Int gridPosition, CardinalPoint enterTileToward)
    {
        _lastCardinalPoint = enterTileToward;

        Tile tile = DirectionToDeadendTileBase(enterTileToward);
        _gooTilemap.SetTile(gridPosition, tile);
        _gooTilemap.RefreshTile(gridPosition);

        _tilesHistory.Add(new TileHistory(gridPosition, enterTileToward, CardinalPoint.None, tile));
        _visitedTiles.Add(gridPosition);

        if(_tilesHistory.Count > 1)
        {
            TileHistory tileHistory = _tilesHistory[_tilesHistory.Count - 2];
            tileHistory.ExitTileToward = enterTileToward;
            _tilesHistory[_tilesHistory.Count - 2] = tileHistory;
        }
    }
    private void SetOldTiles(CardinalPoint cardinalPoint)
    {
        int index = _tilesHistory.Count - 1;

        CardinalPoint enterTo = _tilesHistory[index - 1].EnterTileToward;
        CardinalPoint exitTo = _tilesHistory[index - 1].ExitTileToward;

        if (_pathLookupBig.TryGetValue((enterTo, exitTo), out Tile bigTile))
        {
            TileHistory tileHistory = _tilesHistory[index - 1];
            tileHistory.TileUsed = bigTile;
            _gooTilemap.SetTile(tileHistory.TilePos, bigTile);
            _gooTilemap.RefreshTile(tileHistory.TilePos);
        }

        if (index >= 2)
        {
            enterTo =_tilesHistory[index - 2].EnterTileToward;
            exitTo =_tilesHistory[index - 2].ExitTileToward;
            if (_pathLookupSmall.TryGetValue((enterTo, exitTo), out Tile smallTile))
            {
                TileHistory tileHistory = _tilesHistory[index - 2];
                tileHistory.TileUsed = smallTile;
                _gooTilemap.SetTile(tileHistory.TilePos, smallTile);
                _gooTilemap.RefreshTile(tileHistory.TilePos);
            }
        }

    }
    public void HandleMovementChanges(Vector3Int oldGridPosition, Vector3Int newGridPosition, Vector3 direction)
    {
        CardinalPoint cardinalPoint = Vector3ToCardinalPoint(direction);
        SetCurrentTile(newGridPosition, cardinalPoint);
        SetOldTiles(cardinalPoint);

        if (_endTilemap.HasTile(newGridPosition))
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
        //StartCoroutine(UndoPathCR());
    }

    //private IEnumerator UndoPathCR()
    //{
    //    while (_tilesHistory.Count > 1)
    //    {
    //        var currentPos = _tilesHistory.Peek();
    //        _pathTilemap.SetTile(currentPos.TilePos, _pathTile);
    //        _tilesHistory.Pop();

    //        var lastPos = _tilesHistory.Peek();
    //        Tile tile = DirectionToDeadendTileBase(lastPos.CardinalPoint);
    //        _pathTilemap.SetTile(lastPos.TilePos, tile);
    //        _lastCardinalPoint = lastPos.CardinalPoint;

    //        if(_tilesHistory.Count == 1)
    //        {
    //            _visitedTiles.Add(lastPos.TilePos);
    //        }

    //        yield return new WaitForSeconds(_undoDelay);
    //    }
    //}

    private Tile DirectionToDeadendTileBase(CardinalPoint direction)
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
