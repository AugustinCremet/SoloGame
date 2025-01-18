using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FallInHole : MonoBehaviour
{
    private Tilemap holeTilemap; // Assign the Tilemap containing holes
    private float maxPullStrength = 20f; // Maximum pull force
    private float pullIncreaseRate = .2f; // How fast the pull gets stronger
    private float pullRadius = .5f; // Radius to search for holes
    private float escapeResistance = 0.2f; // Resistance when trying to escape (slows movement)
    private float escapeForceThreshold = 0.1f; // Minimum force required to escape

    private Transform player;
    private Rigidbody2D playerRb;
    private float currentPullStrength = 0f; // Dynamic pull force

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerRb = player.GetComponent<Rigidbody2D>();
        holeTilemap = GetComponent<Tilemap>();
    }

    void FixedUpdate()
    {
        if (player == null || playerRb == null) return;

        Vector3Int playerTilePos = holeTilemap.WorldToCell(player.position);
        Vector3Int nearestHoleTilePos = FindNearestHoleTile(playerTilePos);

        if (nearestHoleTilePos == Vector3Int.zero) return; // No hole found nearby

        Vector3 holeCenter = holeTilemap.GetCellCenterWorld(nearestHoleTilePos);
        Vector3 pullDirection = (holeCenter - player.position).normalized;

        // Gradually increase pull force
        currentPullStrength = Mathf.Min(currentPullStrength + pullIncreaseRate * Time.fixedDeltaTime, maxPullStrength);

        // Apply direct velocity override for a strong pull
        Vector2 newVelocity = playerRb.velocity + (Vector2)(pullDirection * currentPullStrength);
        // Apply escape resistance
        ApplyEscapeResistance(ref newVelocity, pullDirection);
        playerRb.velocity = newVelocity;
    }

    // Find the nearest hole tile around the player
    private Vector3Int FindNearestHoleTile(Vector3Int playerTilePos)
    {
        Vector3Int closestHoleTile = Vector3Int.zero;
        float minDistance = pullRadius + 0.5f;

        // Check nearby tiles within pullRadius
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int checkTile = new Vector3Int(playerTilePos.x + x, playerTilePos.y + y, playerTilePos.z);
                if (holeTilemap.HasTile(checkTile))
                {
                    float distance = Vector3.Distance(player.position, holeTilemap.GetCellCenterWorld(checkTile));
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
            if (velocity.magnitude > escapeForceThreshold)
            {
                velocity *= escapeResistance; // Allow the player to escape, but slow them down
            }
        }
    }
}
