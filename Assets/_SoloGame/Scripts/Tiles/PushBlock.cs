using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PushBlock : MonoBehaviour, IUniqueIdentifier, IDamageable
{
    [SerializeField] Tilemap _pathTilemap;
    [SerializeField] Tilemap[] _obstacleTilemaps;
    [SerializeField] private string _uniqueID;
    public string UniqueID { get => _uniqueID; set => _uniqueID = value; }

    public static List<PushBlock> AllBoxes = new List<PushBlock>();
    public static event Action<PushBlock> OnBoxPushed;
    private bool _isTileBlocked = false;
    private bool _isMoving = false;

    [SerializeField] private int _hp = 200;

    private void Awake()
    {
        AllBoxes.Add(this);
    }

    private void OnDestroy()
    {
        AllBoxes.Remove(this);
    }
    public void StartPushing(Vector2 dir, float duration)
    {
        if (_isMoving)
            return;

        Vector3Int gridPos = _pathTilemap.WorldToCell(transform.position + (Vector3)dir);

        if(!_isTileBlocked && _pathTilemap.HasTile(gridPos))
        {
            StartCoroutine(PushCR(dir, duration));
        }
    }

    public bool IsBoxAtGrid(Vector3Int gridPos)
    {
        foreach(var box in AllBoxes)
        {
            Vector3Int boxGrid = _pathTilemap.WorldToCell(box.transform.position);

            if(boxGrid == gridPos)
                return true;
        }

        return false;
    }

    public bool CanItBePushed(Vector2 dir)
    {
        Vector3Int gridPos = _pathTilemap.WorldToCell(transform.position + (Vector3)dir);

        foreach (var tilemap in _obstacleTilemaps)
        {
            if (tilemap.HasTile(gridPos))
            {
                _isTileBlocked = true;
                return false;
            }
        }

        if (IsBoxAtGrid(gridPos))
            return false;

        return true;
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
        OnBoxPushed?.Invoke(this);
        _isMoving = false;
    }

    public bool IsMoving()
    {
        return _isMoving;
    }

    public void ResetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void Damage(int dmgAmount, Vector2? hitLocation = null, float force = 0)
    {
        _hp -= dmgAmount;

        if(_hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Hit(BulletPro.Bullet bullet, Vector3 position)
    {
        int damageAmount = bullet.moduleParameters.GetInt("Damage");
        float knockForce = bullet.moduleParameters.GetFloat("KnockForce");
        Damage(damageAmount, position, knockForce);
    }
}
