using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class FallInHole : MonoBehaviour
{
    private Tilemap _holeTilemap;
    private float _maxPullStrength = 15f;
    private float _pullIncreaseRate = 1f;
    private float _pullRadius = .5f;
    private float _escapeResistance = 0.2f;
    private float _escapeForceThreshold = 0.1f;
    private bool _isPlayerNear;
    private bool _isPlayerDoneFalling;

    private Transform _player;
    private Rigidbody2D _playerRb;
    private float _currentPullStrength = 1f;
    private float _startingPullStrenght = 1f;

    [SerializeField] bool _isBottomFloor;
    [SerializeField] SceneField _bottomScene;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerRb = _player.GetComponent<Rigidbody2D>();
        _holeTilemap = GetComponent<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerFeet") && !_player.gameObject.GetComponent<Player>().IsInvinsible)
        {
            _isPlayerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerFeet"))
        {
            _isPlayerNear = false;
            _currentPullStrength = _startingPullStrenght;
        }
    }

    void FixedUpdate()
    {
        if (_player == null || _playerRb == null || !_isPlayerNear || _isPlayerDoneFalling) return;

        Vector3Int playerTilePos = _holeTilemap.WorldToCell(_player.position);
        Vector3Int nearestHoleTilePos = FindNearestHoleTile(playerTilePos);

        if (nearestHoleTilePos == Vector3Int.zero) return; // No hole found nearby

        Vector3 holeCenter = _holeTilemap.GetCellCenterWorld(nearestHoleTilePos);
        Vector3 pullDirection = (holeCenter - _player.position).normalized;

        if (Vector3.Distance(holeCenter, _player.position) <= 0.3f)
        {
            _isPlayerDoneFalling = true;
            _playerRb.linearVelocity = Vector2.zero;

            if(_isBottomFloor)
            {
                MySceneManager.Instance.LoadNewLevel(_bottomScene.SceneName, LevelType.Gameplay);
                return;
            }

            _player.position = MySceneManager.Instance.GetCheckpointPosition();
            _isPlayerDoneFalling = false;
            return;
        }

        float movementDot = Vector2.Dot(_playerRb.linearVelocity.normalized, pullDirection);

        if (movementDot > 0)
        {
            _currentPullStrength = Mathf.Min(_currentPullStrength + _pullIncreaseRate * Time.fixedDeltaTime, _maxPullStrength);
        }
        else
        {
            _currentPullStrength = Mathf.Max(_startingPullStrenght, _currentPullStrength - (_pullIncreaseRate * 0.5f * Time.fixedDeltaTime));
        }

        Vector2 newVelocity = _playerRb.linearVelocity + (Vector2)(pullDirection * _currentPullStrength);
        ApplyEscapeResistance(ref newVelocity, pullDirection);
        _playerRb.linearVelocity = newVelocity;
    }

    // Find the nearest hole tile around the player
    private Vector3Int FindNearestHoleTile(Vector3Int playerTilePos)
    {
        Vector3Int closestHoleTile = Vector3Int.zero;
        float minDistance = _pullRadius + 0.5f;

        // Check nearby tiles within pullRadius
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int checkTile = new Vector3Int(playerTilePos.x + x, playerTilePos.y + y, playerTilePos.z);
                if (_holeTilemap.HasTile(checkTile))
                {
                    float distance = Vector3.Distance(_player.position, _holeTilemap.GetCellCenterWorld(checkTile));
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestHoleTile = checkTile;
                    }
                }
            }
        }

        return closestHoleTile;
    }

    private void ApplyEscapeResistance(ref Vector2 velocity, Vector3 pullDirection)
    {
        float dotProduct = Vector3.Dot(velocity.normalized, pullDirection);

        // If player moves against the pull direction, apply escape resistance, but not too strong
        if (dotProduct < 0)
        {
            if (velocity.magnitude > _escapeForceThreshold)
            {
                velocity *= _escapeResistance; // Allow the player to escape, but slow them down
            }
        }
    }
}
