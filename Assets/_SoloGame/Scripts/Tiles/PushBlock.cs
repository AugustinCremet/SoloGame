using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PushBlock : MonoBehaviour
{
    [SerializeField] Tilemap _pathTilemap;
    [SerializeField] Tilemap[] _obstacleTilemaps;

    private bool _isTileBlocked = false;
    private bool _isMoving = false;
    public void StartPushing(Vector2 dir, float duration)
    {
        if (_isMoving)
            return;

        Vector3Int gridPos = _pathTilemap.WorldToCell(transform.position + (Vector3)dir);

        foreach(var tilemap in _obstacleTilemaps)
        {
            if(tilemap.HasTile(gridPos))
            {
                _isTileBlocked = true;
                break;
            }
        }

        if(!_isTileBlocked && _pathTilemap.HasTile(gridPos))
        {
            StartCoroutine(PushCR(dir, duration));
        }
    }

    private IEnumerator PushCR(Vector3 dir, float duration)
    {
        _isMoving = true;
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + dir;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(timeElapsed / duration);
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        transform.position = endPos;
        _isMoving = false;
    }

    public bool IsMoving()
    {
        return _isMoving;
    }
}
