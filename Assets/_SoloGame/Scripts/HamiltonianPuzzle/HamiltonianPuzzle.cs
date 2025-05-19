using UnityEngine;
using UnityEngine.Tilemaps;

public class HamiltonianPuzzle : MonoBehaviour
{
    [SerializeField] Tilemap _puzzleTilemap;
    [SerializeField] Tilemap _startTilemap;
    [SerializeField] Tilemap _endTilemap;
    private GameObject _player;
    [SerializeField] Color _highlightColor;
    [SerializeField] TileBase _filledTile;
    [SerializeField] TileBase _emptyTile;
    [SerializeField] AnimatedTile _failTile;

    private Vector3Int lastCell;
    private int _tileAmount;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("PlayerGoo");
        BoundsInt bounds = _puzzleTilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (_puzzleTilemap.HasTile(pos))
            {
                _puzzleTilemap.SetTileFlags(pos, TileFlags.None);
                _tileAmount++;
            }
        }
    }
    void Update()
    {
        int playerX = Mathf.FloorToInt(_player.transform.position.x);
        int playerY = Mathf.FloorToInt( _player.transform.position.y);
        Vector3Int playerWorldPos = new Vector3Int(playerX, playerY, 0);
        Vector3Int cellPos = _puzzleTilemap.WorldToCell(playerWorldPos);

        // Verify if the current tile is part of the puzzle
        if (cellPos != lastCell && _puzzleTilemap.HasTile(cellPos))
        {
            // Verify if the tile should be changed
            if(_puzzleTilemap.GetTile(cellPos) == _filledTile)
            {
                Debug.Log("restart");
                _puzzleTilemap.SetTile(cellPos, _failTile);
                _puzzleTilemap.RefreshTile(cellPos);
            }
            else
            {
                Debug.Log(_puzzleTilemap.GetTile(cellPos));
                _puzzleTilemap.SetTile(cellPos, _filledTile);
                lastCell = cellPos;
                float scaleRatio = 1f / _tileAmount / 2f;
                _player.transform.localScale = _player.transform.localScale - new Vector3(scaleRatio, scaleRatio, scaleRatio);
            }
        }
        if(_startTilemap.HasTile(cellPos))
        {
            Debug.Log("Starting tile");
        }
        if(_endTilemap.HasTile(cellPos))
        {
            Debug.Log("Ending tile");
        }
    }

    private bool CheckIfWon()
    {
        BoundsInt bounds = _puzzleTilemap.cellBounds;
        bool isWon = true;

        foreach(var pos in bounds.allPositionsWithin)
        {
            TileBase tile = _puzzleTilemap.GetTile(pos);
            if (tile != null)
            {
                Color color = _puzzleTilemap.GetColor(pos);
                if(color != _highlightColor)
                {
                    Debug.Log("Restart");
                    isWon = false;
                }
            }
        }

        return isWon;
    }
}
