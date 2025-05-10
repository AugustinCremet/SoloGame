using UnityEngine;
using UnityEngine.Tilemaps;

public class HamiltonianPuzzle : MonoBehaviour
{
    [SerializeField] Tilemap _tilemap;
    private GameObject _player;
    [SerializeField] Color _highlightColor;

    private Vector3Int lastCell;


    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("PlayerGoo");
    }
    void Update()
    {
        int playerX = Mathf.FloorToInt(_player.transform.position.x);
        int playerY = Mathf.FloorToInt( _player.transform.position.y);
        Vector3Int playerWorldPos = new Vector3Int(playerX, playerY, 0);
        Vector3Int cellPos = _tilemap.WorldToCell(playerWorldPos);

        if (cellPos != lastCell)
        {
            if(_tilemap.GetColor(cellPos) == _highlightColor)
            {
                Debug.Log("restart");
            }
            _tilemap.SetTileFlags(cellPos, TileFlags.None);
            _tilemap.SetColor(cellPos, _highlightColor);
            lastCell = cellPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerGoo"))
        {
            CheckIfWon();
        }
    }

    private bool CheckIfWon()
    {
        BoundsInt bounds = _tilemap.cellBounds;
        bool isWon = true;

        foreach(var pos in bounds.allPositionsWithin)
        {
            TileBase tile = _tilemap.GetTile(pos);
            if (tile != null)
            {
                Color color = _tilemap.GetColor(pos);
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
